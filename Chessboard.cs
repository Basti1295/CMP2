using UnityEngine;
using System.Collections;

public class Chessboard : MonoBehaviour
{

	public int m_iSize = 10;
	GameObject[,] m_Grid;

	bool GetAlive(int _iCol, int _iRow)
	{
		return (m_Grid [ _iCol,  _iRow].GetComponent<Renderer> ().material.color == Color.black);
	}

	void SetAlive(int _iCol, int _iRow, bool _bAlive){
		if (_balive) {
			m_Grid [_iCol, _iRow].GetComponent<Renderer> ().material.color = Color.black;
		}
		else
			m_Grid [_iCol, _iRow].GetComponent<Renderer> ().material.color = Color.white;
	}


	void ToggleMouseField (){
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		int xIndex = (int)mouseWorldPos.x;
		int yIndex = (int)mouseWorldPos.y;
		//if(mousePosition == m_Grid
		if (xIndex >= 0 && xIndex < m_iSize && yIndex >= 0 && yIndex < m_iSize)
			Toggle (xIndex, yIndex);

	}


	void Toggle(int _iCol, int _iRow){
				if (GetAlive( _iCol, _iRow)) {
					SetAlive ( _iCol, _iRow, false);
				} 
				else {
					SetAlive ( _iCol, _iRow, true);
				
				}
			}
		


	//Töte alle lebenden Kacheln mit Taste "k"
	void KillAll()
	{
			for (int iCol = 0; iCol < m_iSize; iCol++) {
				for (int iRow = 0; iRow < m_iSize; iRow++) {
				SetAlive (iCol, iRow, false);
				}
			}
		}

	int GetAliveNeighbours(int _iColumn,int _iRow)
	{
		int iAliveNeighbours = 0;
		//Range in for-Schleifen
		for (int iCol = _iColumn-1; iCol <= _iColumn+1; iCol++) {
			for (int iRow = _iRow-1; iRow <= _iRow+1; iRow++) {

				if((iCol == _iColumn && iRow == _iRow) == false && // dont check self 
				   	iCol >= 0 && iCol < m_iSize && iRow >= 0 && iRow < m_iSize && //check bounds
				   GetAlive( iCol, iRow)){
					iAliveNeighbours ++;
				}
			}
		}
		// alternativ zu dont check yourself:
		// if(SetAlive(_iColumn, _iRow, true)){
		// iAliveNeighbours --; (Alex's Lösung)

		return iAliveNeighbours;
	}

	// Use this for initialization
	void Start ()
	{


		m_Grid = new GameObject[m_iSize, m_iSize];

		for (int i = 0; i < m_iSize; i++) {
			for (int j = 0; j < m_iSize; j++) {
				GameObject kachel = GameObject.CreatePrimitive (PrimitiveType.Quad);
				m_Grid [i, j] = kachel;

				kachel.name = "Kachel(" + i + "," + j + ")";

				float r = Random.value;
				if (r < 0.5f) {
					SetAlive (i, j, true);
			
				}

				kachel.transform.position = new Vector3 (i + 0.5f, j + 0.5f, 0);
				kachel.transform.parent = this.transform;
			}
		}
		Camera.main.transform.position = new Vector3 (m_iSize / 2, m_iSize / 2, -10);
		Camera.main.orthographicSize = m_iSize;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			ToggleMouseField();
		}


		if (Input.GetKeyDown (KeyCode.K)){
			KillAll(); //Töte alle lebenden Kacheln
		}

		// Drücke Leertaste um Veränderung zu beobachten
		if (Input.GetKeyDown (KeyCode.Space) == false)
			return;


		// 2D Array das die lebenden Nachbarn jeder Kachel speichert
		int [,] Alive = new int[m_iSize, m_iSize];
			for (int iCol = 0; iCol < m_iSize; iCol++){
				for (int iRow = 0; iRow < m_iSize; iRow++){
					Alive [iCol, iRow] = GetAliveNeighbours (iCol, iRow);
			}
		}


		for (int iCol = 0; iCol < m_iSize; iCol++) {
			for (int iRow = 0; iRow < m_iSize; iRow++) {
			
				int iAliveNeighbours = Alive [iCol, iRow];

				// tile dead
				if (GetAlive(iCol, iRow) == false) {
					if (iAliveNeighbours == 3) {
						SetAlive (iCol, iRow, true);
					}
				}
				// tile alive
				else {
					if (iAliveNeighbours < 2 || iAliveNeighbours > 3) {
						SetAlive (iCol, iRow, false);
					}
				}
			}

		}
			
		
		print ("Anzahl Nachbarn: " + GetAliveNeighbours (1, 1));
	}
}