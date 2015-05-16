namespace JIAO.JIAO
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class JIAOGraphicController : MonoBehaviour
    {
		public RenderTexture rendertexturevelocityBase;            
		public RenderTexture rendertexturedensityBase;  
		public RenderTexture rendertexturevelocity;            
		public RenderTexture rendertexturedensity;

		public Player player;
       

		public Shader			fluidShader;				//  Shader for render
		public Shader			velocityShader;
		public Material 		m_mat;						//  Material for render

		public RenderTexture	velocityDisdpalyGrid;		//  The RenderTexture for simulation velocity.
		public Rect             simulationSize = new Rect(0,0,512,512);     //  A rect for the size of the simulation grid.
		public float           	m_ddx;				//	step dx in uv space (length of a pixel in uv)
		public float           	m_ddy;				//	step dy	in uv space
		
		//public Texture2D		noise;				//	noise map
		public Color			brushColor;			//  brush color
		public float            G = 1;
		public int				lodLevel;
		
		// Parameters about the input 
		public Vector3          brushPosition = new Vector3(0, 0, 0);
		private Vector3         m_lastBrushPosition = new Vector3(0, 0, 0);
		public Vector3			screenVelcity;
		public float 			brushRadius = 10;
		
		// Parameters about the fluid
		public float 			diffusionRate = 1;				//	Control the diffusion rate of the MATERIAL in fluid
		public float 			velocityDiffusionRate = 1;		//	Control the diffusion rate of the fluid VELOCITY
		
		private bool			m_Init = false;
		private bool			m_isFieldForceOn = false;


        public void Start ()
        {
			if(fluidShader){
				m_mat = new Material(fluidShader);
			}
			else return;//shader not set or other error;


			
			//Create buffer ---->
			simulationSize.width = Screen.width;
			simulationSize.height = Screen.height;
			rendertexturevelocity = new RenderTexture((int)simulationSize.width, (int)simulationSize.height, 0, RenderTextureFormat.ARGBFloat);
			rendertexturevelocity.anisoLevel = 8;
			//rendertexturevelocity.enableRandomWrite = true;
			rendertexturevelocity.wrapMode = TextureWrapMode.Clamp;
			rendertexturevelocity.filterMode = FilterMode.Bilinear;
			rendertexturevelocity.Create();
			if(rendertexturevelocity.IsCreated() == false){
				Debug.Log("Velocity Tetxure created failed.");
				this.enabled = false;
			}
			
			rendertexturedensity = new RenderTexture((int)simulationSize.width, (int)simulationSize.height, 0, RenderTextureFormat.ARGBFloat);
			rendertexturedensity.anisoLevel = 8;
			rendertexturedensity.useMipMap = true;
			rendertexturedensity.generateMips = true;
			//rendertexturedensity.enableRandomWrite = true;
			rendertexturedensity.wrapMode = TextureWrapMode.Clamp;
			rendertexturedensity.filterMode = FilterMode.Bilinear;
			rendertexturedensity.Create();
			if(rendertexturedensity.IsCreated() == false){
				Debug.Log("Density Tetxure created failed.");
				this.enabled = false;
			}

			rendertexturevelocityBase = new RenderTexture((int)simulationSize.width, (int)simulationSize.height, 0, RenderTextureFormat.ARGBFloat);
			rendertexturevelocityBase.anisoLevel = 8;
			//rendertexturevelocity.enableRandomWrite = true;
			rendertexturevelocityBase.wrapMode = TextureWrapMode.Clamp;
			rendertexturevelocityBase.filterMode = FilterMode.Bilinear;
			rendertexturevelocityBase.Create();
			if(rendertexturevelocityBase.IsCreated() == false){
				Debug.Log("Velocity Tetxure created failed.");
				this.enabled = false;
			}
			
			rendertexturedensityBase = new RenderTexture((int)simulationSize.width, (int)simulationSize.height, 0, RenderTextureFormat.ARGBFloat);
			rendertexturedensityBase.anisoLevel = 8;
			rendertexturedensityBase.useMipMap = true;
			rendertexturedensityBase.generateMips = true;
			//rendertexturedensity.enableRandomWrite = true;
			rendertexturedensityBase.wrapMode = TextureWrapMode.Clamp;
			rendertexturedensityBase.filterMode = FilterMode.Bilinear;
			rendertexturedensityBase.Create();
			if(rendertexturedensityBase.IsCreated() == false){
				Debug.Log("Density Tetxure created failed.");
				this.enabled = false;
			}

			velocityDisdpalyGrid = new RenderTexture((int)simulationSize.width, (int)simulationSize.height, 0, RenderTextureFormat.ARGBFloat);
			velocityDisdpalyGrid.anisoLevel = 8;
			velocityDisdpalyGrid.wrapMode = TextureWrapMode.Clamp;
			velocityDisdpalyGrid.filterMode = FilterMode.Point;
			velocityDisdpalyGrid.Create();
			if(velocityDisdpalyGrid.IsCreated() == false){
				Debug.Log("velocityDisplay Tetxure created failed.");
				this.enabled = false;
			}

			GameObject.Find("RTCam").GetComponent<Camera>().targetTexture = rendertexturedensityBase;
			GameObject.Find("RTFieldCam").GetComponent<Camera>().targetTexture = rendertexturevelocityBase;
			GameObject.Find("RTFieldCam").GetComponent<Camera>().SetReplacementShader(velocityShader,"RenderType");
			// Set pixel step for x & y ---->
			m_ddx = 1 / simulationSize.width;
			m_ddy = 1 / simulationSize.height;
			// <----
			m_Init = false;
			//	Clear Buffer
			Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,7);
			Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,0);
			// Set experiment environment
			Screen.SetResolution(800,800,false);
			m_mat.SetTexture("velocityGridBase",rendertexturevelocityBase);
			m_mat.SetTexture("inkGridBase",rendertexturedensityBase);
			SetParameters();
            
        }

		static public void Blit(RenderBuffer COLOR,RenderBuffer DEPTH, Material MRTMat,int pass)
		{
			Graphics.SetRenderTarget(COLOR, DEPTH);
			RenderSSQuad (MRTMat, pass);
		}

		static public void RenderSSQuad( Material MRTMat,int pass)
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			MRTMat.SetPass(pass);
			GL.Begin(GL.QUADS);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.0f, 1.0f, 0.1f);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.1f);		
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 0.0f, 0.1f);		
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.1f);		
			GL.End();
			GL.PopMatrix();
		}

		void Update() {
			
			m_lastBrushPosition = brushPosition;

			SetParameters();
			m_mat.SetVector("screenVelcity",new Vector4(player.m_velocity.x,player.m_velocity.y,player.m_velocity.z, 1f));
			if(player.m_maincam.WorldToScreenPoint(player.m_pos.position).x > Screen.width / 2) {
			}

			// Press space to clear buffer
			if(Input.GetKeyUp(KeyCode.Space)) {
				Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,7);	
				Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,0);
				Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,16);
				m_Init = true;
			}
			
			//	Press V to create a constant force field at the bottom of the simulation area.
			if(Input.GetKeyUp(KeyCode.V)){
				m_isFieldForceOn = !m_isFieldForceOn;
			}
			
			//	Add material(Ink) on Canvas, and add velocity

			Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,3);
			Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,1);

			
			//	Add velocity
			//if(Input.GetMouseButton(1)){
			//	Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,3);
			//}
			
			//	Compute Material tension


			Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,10);
			
			//	Simulate Advection 
			Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,8);
			
			// Calculate the divergence of velocity filed 
			Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,11);
			
			//	Calculate pressure using jacobi iteration method
			for(int i = 0 ; i <50; ++i) {
				Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,12);
			}
			
			//add pressure
			Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,13);
			
			// Add force to the field
			if(m_isFieldForceOn){
				Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,15);
			}
			
			else{	//	Handle boundary situation ( No fluid should leak the simulation space )
				Blit(rendertexturevelocity.colorBuffer, rendertexturevelocity.depthBuffer, m_mat,14);
			}
			
			if(Input.GetKeyUp(KeyCode.A)){
				Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,17);
			}

			
			//	Diffuse and advect Ink based on the velocity filed.
			Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,4);
			Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,2);
			//Blit(rendertexturedensity.colorBuffer, rendertexturedensity.depthBuffer, m_mat,16);
			Blit(velocityDisdpalyGrid.colorBuffer, velocityDisdpalyGrid.depthBuffer, m_mat,5);
		}
		void SetParameters () {
			
			//diffusionRate = Mathf.Clamp(diffusionRate,0,0.2f);
			m_mat.SetFloat("dx",m_ddx);
			m_mat.SetFloat("dy",m_ddy);
			m_mat.SetTexture("simulationGrid",rendertexturedensity);
			m_mat.SetVector("mouse",brushPosition);
			m_mat.SetFloat("screenWidth",simulationSize.width);
			m_mat.SetFloat("screenHeight",simulationSize.height);
			m_mat.SetFloat("brushRadius",brushRadius);
			m_mat.SetFloat("diffuseRate", diffusionRate);
			m_mat.SetFloat("diffuseRate_vel", velocityDiffusionRate);
			//m_mat.SetTexture("noise",noise);
			
			m_mat.SetTexture("velocityGrid",rendertexturevelocity);
			m_mat.SetTexture("velocityDisdpalyGrid",velocityDisdpalyGrid);
			m_mat.SetInt("IsAdd",Input.GetMouseButton(0) ? 1 : 0);
			//m_mat.SetVector("screenVelcity",screenVelcity);
			m_mat.SetVector("brushColor",brushColor);		
			m_mat.SetInt("lodLevel",lodLevel);
			m_mat.SetFloat("G",G);
		}

    }
}
