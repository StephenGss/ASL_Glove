using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class fingers : MonoBehaviour {

    SerialPort s = new SerialPort("COM4", 9600); //Set the port (com4) and the baud rate (9600, is standard on most devices)
    float[] data = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Need the last values to moves fingers
    float[] ldata = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Need the last values to moves fingers
    float[] hdata = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Need the last values to moves fingers
    float[] offset = { -30, -100, -90, -10, -110, 0, 0, 0, 0, 0, 0 };

    int[,] character_data = new int[,] {{400,475,350,400,300,350,200,250,250,275,0,0,1,0,0,0,0,0,0},
                                        {300,350,525,550,500,550,450,500,500,525,0,0,1,0,0,0,0,0,0},
                                        {375,425,475,500,400,450,325,400,425,450,0,0,1,0,0,0,0,0,0},
                                        {325,375,500,550,375,400,275,300,325,375,1,0,0,0,0,0,0,0,0},
                                        {300,325,400,425,350,375,275,300,300,325,0,0,-1,0,0,0,0,0,0},
                                        {375,400,400,425,500,525,425,475,475,525,0,0,0,0,0,0,0,0,0},
                                        {425,475,475,500,325,375,225,275,275,300,0,0,0,-1,20,1,20,-1,20},
                                        {300,375,500,550,450,500,225,250,275,350,0,0,1,-1,25,1,20,-1,25},
                                        {300,350,375,400,350,375,250,275,450,500,1,1,1,1,20,-1,25,-1,25},
                                        {300,350,375,400,350,375,250,275,450,500,1,1,1,-1,10,1,20,-1,25},
                                        {475,525,500,525,425,450,250,275,300,325,0,0,0,1,20,-1,25,-1,25},
                                        {550,575,525,550,325,350,225,250,275,325,0,0,0,0,0,0,0,0,0},
                                        {325,375,375,400,325,375,250,325,350,400,0,0,1,0,0,0,0,0,0},
                                        {375,425,375,400,375,400,250,300,275,350,0,0,1,0,0,0,0,0,0},
                                        {325,350,400,425,375,425,300,325,350,375,1,0,1,0,0,0,0,0,0},
                                        {425,450,500,525,425,450,250,275,325,350,0,0,0,-1,0,1,0,-1,0},
                                        {450,500,475,500,350,375,225,250,275,300,0,-1,0,-1,0,1,0,-1,0},
                                        {350,375,500,525,450,475,275,300,325,350,0,1,0,0,0,0,0,0,0},
                                        {325,350,350,400,325,350,225,250,275,300,-1,0,1,0,0,0,0,0,0},
                                        {400,450,400,450,350,400,250,275,300,350,0,0,0,0,0,0,0,0,0},
                                        {350,375,500,550,475,525,275,300,350,375,0,0,1,1,25,-1,25,-1,25},
                                        {325,400,525,575,500,525,250,300,325,350,0,0,0,0,0,0,0,0,0},
                                        {300,375,500,550,500,550,425,475,275,350,0,0,0,0,0,0,0,0,0},
                                        {300,350,425,475,325,350,225,275,275,300,1,0,0,0,0,0,0,0,0},
                                        {550,575,375,400,350,375,250,275,475,525,0,0,1,0,0,0,0,0,0},
                                        {325,375,500,525,325,350,250,275,275,300,-1,0,0,-1,12,1,20,-1,0}};
    string[] dictionary = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    string letter = "-";
    int letterIndex = 0;
    

    // Use this for initialization
    void Start () {
        print("initializing...");
        s.ReadBufferSize = 9600;
        s.DataBits = 8;
        s.ReadTimeout = 10;
        s.Open(); //Open the Serial Stream.

        GameObject right_hand = GameObject.Find("right_hand");
        right_hand.SetActive(true);

    }
	
	// Update is called once per frame
	void Update () {
        string value;
        try
        {
            value = s.ReadLine(); //Read the information
        }catch(TimeoutException e)
        {
            value = "-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1";
        }
        string[] readin = value.Split(','); //values are comma delimited
        bool cont = true;

        try
        {
            for (int i = 0; i < 11; i++) //Check if all values are recieved
            {
                if (readin[i] == "" || readin[i] == "-1")
                    cont = false;
            }
        }catch(IndexOutOfRangeException e)
        {
            cont = false;
        }
        

        if (cont) //only continue if we have all values
        {
            
            data[0] = float.Parse(readin[0]) + offset[0];  //Set new values to last time values for the next loop
            data[1] = float.Parse(readin[1]) + offset[1];
            data[2] = float.Parse(readin[2]) + offset[2];  
            data[3] = float.Parse(readin[3]) + offset[3];  
            data[4] = float.Parse(readin[4]) + offset[4];
            data[5] = float.Parse(readin[5]) + offset[5];
            data[6] = float.Parse(readin[6]) + offset[6];  
            data[7] = float.Parse(readin[7]) + offset[7];
            data[8] = float.Parse(readin[8]) + offset[8];
            data[9] = float.Parse(readin[9]) + offset[9];  
            data[10] = float.Parse(readin[10]) + offset[10];

            print(readin[5] + readin[6] + readin[7]);
            /*
            var thumb = GameObject.Find("thumb");
            thumb.transform.rotation = Quaternion.Euler(-60, 0, -2 * (data[0] / 4 + 50));
            var index = GameObject.Find("index");
            index.transform.rotation =  Quaternion.Euler(0, 0, -2 * (data[1] / 4 + 50));
            var middle = GameObject.Find("middle");
            middle.transform.rotation = Quaternion.Euler(0, 0, -2 * (data[2] / 4 + 50));
            var ring = GameObject.Find("ring");
            ring.transform.rotation = Quaternion.Euler(0, 0, -2 * (data[3] / 4 + 50));
            var pinky = GameObject.Find("pinky");
            pinky.transform.rotation = Quaternion.Euler(0, 0, -2 * (data[4] / 4 + 50));
            */

            var thumb = GameObject.Find("fingerbase_004_R");
            thumb.transform.rotation = Quaternion.Euler(-60, 0, -2 * (data[0] / 4 + 50));
            var index = GameObject.Find("fingerbase_000_R");
            index.transform.rotation = Quaternion.Euler(0, 0, -2 * (data[1] / 4 + 50));
            var middle = GameObject.Find("fingerbase_001_R");
            middle.transform.rotation = Quaternion.Euler(0, 0, -2 * (data[2] / 4 + 50));
            var ring = GameObject.Find("fingerbase_002_R");
            ring.transform.rotation = Quaternion.Euler(0, 0, -2 * (data[3] / 4 + 50));
            var pinky = GameObject.Find("fingerbase_003_R");
            pinky.transform.rotation = Quaternion.Euler(0, 0, -2 * (data[4] / 4 + 50));

            //var wrist = GameObject.Find("wrist");
            //wrist.transform.rotation = Quaternion.Euler(data[6] * 3, data[5] , data[7] * -3 );

            for (int x = 0; x < dictionary.Length; x++)
            {
                if (Input.GetKeyDown(dictionary[x])) {
                    letter = dictionary[x];
                    letterIndex = x;
                }
            }

            if(letterIndex >= 0)
            {
                ldata[0] = character_data[letterIndex,0] + offset[0];  //Set new values to last time values for the next loop
                ldata[1] = character_data[letterIndex, 2] + offset[1];
                ldata[2] = character_data[letterIndex, 4] + offset[2];
                ldata[3] = character_data[letterIndex, 6] + offset[3];
                ldata[4] = character_data[letterIndex, 8] + offset[4];

                hdata[0] = character_data[letterIndex, 1] + offset[0];  //Set new values to last time values for the next loop
                hdata[1] = character_data[letterIndex, 3] + offset[1];
                hdata[2] = character_data[letterIndex, 5] + offset[2];
                hdata[3] = character_data[letterIndex, 7] + offset[3];
                hdata[4] = character_data[letterIndex, 9] + offset[4];
            }
            
            /*
            var thumbl = GameObject.Find("thumbl");
            thumbl.transform.rotation = Quaternion.Euler(-60, 0, -2 * (ldata[0] / 4 + 50));
            var indexl = GameObject.Find("indexl");
            indexl.transform.rotation = Quaternion.Euler(0, 0, -2 * (ldata[1] / 4 + 50));
            var middlel = GameObject.Find("middlel");
            middlel.transform.rotation = Quaternion.Euler(0, 0, -2 * (ldata[2] / 4 + 50));
            var ringl = GameObject.Find("ringl");
            ringl.transform.rotation = Quaternion.Euler(0, 0, -2 * (ldata[3] / 4 + 50));
            var pinkyl = GameObject.Find("pinkyl");
            pinkyl.transform.rotation = Quaternion.Euler(0, 0, -2 * (ldata[4] / 4 + 50));

            var thumbh = GameObject.Find("thumbh");
            thumbh.transform.rotation = Quaternion.Euler(-60, 0, -2 * (hdata[0] / 4 + 50));
            var indexh = GameObject.Find("indexh");
            indexh.transform.rotation = Quaternion.Euler(0, 0, -2 * (hdata[1] / 4 + 50));
            var middleh = GameObject.Find("middleh");
            middleh.transform.rotation = Quaternion.Euler(0, 0, -2 * (hdata[2] / 4 + 50));
            var ringh = GameObject.Find("ringh");
            ringh.transform.rotation = Quaternion.Euler(0, 0, -2 * (hdata[3] / 4 + 50));
            var pinkyh = GameObject.Find("pinkyh");
            pinkyh.transform.rotation = Quaternion.Euler(0, 0, -2 * (hdata[4] / 4 + 50));
            */

            s.BaseStream.Flush(); //Clear the serial information so we assure we get new information.
        }


        

    }

    void OnGUI()
    {
        string newString = (data[0] - offset[0]) + "\n" + 
                            (data[1] - offset[1]) + "\n" + 
                            (data[2] - offset[2]) + "\n" + 
                            (data[3] - offset[3]) + "\n" + 
                            (data[4] - offset[4]) + "\n" + 
                            (data[5] - offset[5]) + "\n" + 
                            (data[6] - offset[6]) + "\n" + 
                            (data[7] - offset[7]) + "\n" + 
                            (data[8] - offset[8]) + ", " + 
                            (data[9] - offset[9]) + ", " + 
                            (data[10] - offset[10] + "\n" +
                            "Selected Letter: " + letter);
        GUI.Label(new Rect(10, 10, 100, 200), newString); //Display new values
                                                          // Though, it seems that it outputs the value in percentage O-o I don't know why.
    }
}
