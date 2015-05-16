/////////////////////////////////////////////////////////////////
/// 2D eulerian Fluid simulation experiment.
///	Author: ZiZi (Razor Yhang 杨宇辰)  709230197@qq.com 
///	This project is a lab-rats project, for acadamic purpose only.
///
///	ZiZi:
///		The equation behind the following code is Navier-Stokes
///	Fluid equation. The equation takes fluid velocity and other
///	fluid properties as input. 
///
/// School of Software Engineering, BJUT
/// 2014 02
/////////////////////////////////////////////////////////////////


/*
	Hello! The programer who take part in the Ink game project :)
	
	To use this 2Dfluid solver, one should send some parameters to the 
	render shader (i.e. "fluidShader.shader "). The fluid is solved by 
	the passes in that shader by a sequences of computation. Regard the simulation
	as a grid with a lot of lattice and each lattice has some properties
	such as velocity / ink density(or material density for general situation) 
	/ Fluid diffusion etc. The solver will calculate the fluid field 
	every frame based on those properties and dicide how fluid flows, aka.
	the velocity of every lattices in the simulation field. After that,
	the material in the fluid, in this case, the ink, will be translated to 
	another place based on the velocity we got from the calculation. 
	This is how the ink flows.

	For utility purpose, the only thing you need to do is sending inputs into the buffer and set some 
	parameters. The most important inputs are VELOCITY and MATERIAL DENSITY.
	Material density determine the "Color" of ink, So if you set one lattice 
	to flot4(0,0,0,1),it means you have "added some pure ink". I have put the material
	density in a RenderTexture called "densityGrid" and wrote a "draw" function 
	in the shader(pass # 1).So if you want to add ink into the canvas,just
	call Blit() and render the the "densityGrid" with pass #1.check those 
	out and you will know how it works. Another important input, Velocity,  
	handle the fluidity of the fluid, which is stored in RenderTexture 
	"velocityGrid". Mathematicaly, this buffer should not be modified because
	the simulation I made is called "Incompressible Newtton Fluid". But we 
	don't have to strickly follow the real world physics, if you want to 
	mannually change the speed of velocity of the certain area in the simulation
	grid, just change it. The method is similar to how to change Material density.

	There are some other parameters like fluid diffusion rate. But I found it
	is hard to tune those value for those who don't know fluid simultion knowledge.
	So I have written a SetParameters() function for parameter setting. 
	Call this function and it will send a lot of value(that I have tuned) into 
	the shader.

	If you have questions, please contact me by email or tell my friend Miss Gao.

	PS: I think it whould be interesting to project the velocity in Wolrd space
	into Screen space by multiply view and projection Matrix. And then use these
	screen space velocity as input. The ink in the simulation area will flow
	as the object moves, which may looks good.

*/	

using UnityEngine;
using System.Collections;

public class Fluid : MonoBehaviour {

	// global parameters 
	public Shader			fluidShader;				//  Shader for render
	public Material 		m_mat;						//  Material for render 
	public RenderTexture    densityGrid;        		//  The RenderTexture for simulation density.
	public RenderTexture	velocityGrid;				//  The RenderTexture for simulation velocity.
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

	// Use this for initialization
	void Start () {
		//create material for render.
		if(fluidShader){
			m_mat = new Material(fluidShader);
		}
		else return;//shader not set or other error;


		//Create buffer ---->
		velocityGrid = new RenderTexture((int)simulationSize.width, (int)simulationSize.height, 0, RenderTextureFormat.ARGBFloat);
		velocityGrid.anisoLevel = 8;
		//velocityGrid.enableRandomWrite = true;
		velocityGrid.wrapMode = TextureWrapMode.Clamp;
		velocityGrid.filterMode = FilterMode.Bilinear;
		velocityGrid.Create();
		if(velocityGrid.IsCreated() == false){
			Debug.Log("Velocity Tetxure created failed.");
			this.enabled = false;
		}

		densityGrid = new RenderTexture((int)simulationSize.width, (int)simulationSize.height, 0, RenderTextureFormat.ARGBFloat);
		densityGrid.anisoLevel = 8;
		densityGrid.useMipMap = true;
		densityGrid.generateMips = true;
		//densityGrid.enableRandomWrite = true;
		densityGrid.wrapMode = TextureWrapMode.Clamp;
		densityGrid.filterMode = FilterMode.Bilinear;
		densityGrid.Create();
		if(densityGrid.IsCreated() == false){
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
		// <----

		GameObject.Find("FluidSolverObj").GetComponent<Renderer>().material.mainTexture = densityGrid;
		GameObject.Find("Velocity").GetComponent<Renderer>().material.mainTexture = velocityDisdpalyGrid;

        // Set pixel step for x & y ---->
        m_ddx = 1 / simulationSize.width;
        m_ddy = 1 / simulationSize.height;
        // <----

		// Set experiment environment
		Screen.SetResolution(800,800,false);

		// Set Shader parameters
		SetParameters();

		//	Clear Buffer
		Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,7);
		Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,0);
		m_Init = false;
	}
	
	// Update is called once per frame
	void Update () {

		//	Set bursh.
        brushPosition.x = Input.mousePosition.x / Screen.width;
        brushPosition.y = Input.mousePosition.y / Screen.height;
		screenVelcity = Vector3.Lerp(brushPosition - m_lastBrushPosition, screenVelcity, .9f);

		m_lastBrushPosition = brushPosition;

		/////////////////////////////////////////////////////
		//Experiment part ---->
		////////////////////////////////////////////////////

		// Set parameters for shaders.
		SetParameters();

		// Press space to clear buffer
		if(Input.GetKeyUp(KeyCode.Space)){
			Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,7);	
			Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,0);
			Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,16);
			m_Init = true;
		}

		//	Press V to create a constant force field at the bottom of the simulation area.
		if(Input.GetKeyUp(KeyCode.V)){
			m_isFieldForceOn = !m_isFieldForceOn;
		}

		//	Add material(Ink) on Canvas, and add velocity
		if(Input.GetMouseButton(0)){
			Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,3);
			Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,1);
		}

		//	Add velocity
		if(Input.GetMouseButton(1)){
			Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,3);
		}

		//	Compute Material tension 
		Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,10);

		//	Simulate Advection 
		Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,8);

		// Calculate the divergence of velocity filed 
		Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,11);

		//	Calculate pressure using jacobi iteration method
		for(int i = 0 ; i <50; ++i){
			Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,12);
		}

		//add pressure
		Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,13);

		// Add force to the field
		if(m_isFieldForceOn){
			Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,15);
		}

		else{	//	Handle boundary situation ( No fluid should leak the simulation space )
			Blit(velocityGrid.colorBuffer, velocityGrid.depthBuffer, m_mat,14);
		}

		if(Input.GetKeyUp(KeyCode.A)){
			Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,17);
		}

		//	Diffuse and advect Ink based on the velocity filed.
		Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,4);
		Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,2);
		//Blit(densityGrid.colorBuffer, densityGrid.depthBuffer, m_mat,16);
		Blit(velocityDisdpalyGrid.colorBuffer, velocityDisdpalyGrid.depthBuffer, m_mat,5);
		/////////////////////////////////////////////////////////
		//<---- End experiment
		/////////////////////////////////////////////////////////
	}

	void OnGUI () {

		if(m_Init == false){
			GUI.color = Color.white;
			GUI.Label(new Rect(Screen.width/2 - 100,Screen.height/2,500,200),"Press Space to start the demo and reset screen.\n" +
				"Hold Left button and drag mouse to add dye.\n" +
				"Hold Right button and drag to add forece.\n" +
			    "Press [V] to change mode and add an extra force.\n" + 
				"ZiZi 2014");
		}

		else{
			//GUI.DrawTexture(new Rect(0,0,800,800),densityGrid);
		}

	}

	void SetParameters () {

		//diffusionRate = Mathf.Clamp(diffusionRate,0,0.2f);
		m_mat.SetFloat("dx",m_ddx);
		m_mat.SetFloat("dy",m_ddy);
		m_mat.SetTexture("simulationGrid",densityGrid);
		m_mat.SetVector("mouse",brushPosition);
		m_mat.SetFloat("screenWidth",simulationSize.width);
		m_mat.SetFloat("screenHeight",simulationSize.height);
		m_mat.SetFloat("brushRadius",brushRadius);
		m_mat.SetFloat("diffuseRate", diffusionRate);
		m_mat.SetFloat("diffuseRate_vel", velocityDiffusionRate);
		//m_mat.SetTexture("noise",noise);

		m_mat.SetTexture("velocityGrid",velocityGrid);
		m_mat.SetTexture("velocityDisdpalyGrid",velocityDisdpalyGrid);
		m_mat.SetInt("IsAdd",Input.GetMouseButton(0) ? 1 : 0);
		m_mat.SetVector("screenVelcity",screenVelcity);
		m_mat.SetVector("brushColor",brushColor);		
		m_mat.SetInt("lodLevel",lodLevel);
		m_mat.SetFloat("G",G);
	}

	//	Render a RT using material MRTMat with a given pass
	//	Note: 	1.pass start with number #0.
	//			2.Send colorbuffer and Depthbuffer into COLOR and DEPTH.
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


	
}
