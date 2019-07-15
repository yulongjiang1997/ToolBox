using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        public static void SaveStreamToFile(Stream FromStream, string TargetFile)
        {
            // FromStream=the stream we wanna save to a file 
            //TargetFile = name&path of file to be created to save to 
            //i.e"c:\mysong.mp3" 
            try
            {
                //Creat a file to save to
                Stream ToStream = File.Create(TargetFile);

                //use the binary reader & writer because
                //they can work with all formats
                //i.e images, text files ,avi,mp3..
                BinaryReader br = new BinaryReader(FromStream);
                BinaryWriter bw = new BinaryWriter(ToStream);

                //copy data from the FromStream to the outStream
                //convert from long to int 
                bw.Write(br.ReadBytes((int)FromStream.Length));
                //save
                bw.Flush();
                //clean up 
                bw.Close();
                br.Close();
            }

            //use Exception e as it can handle any exception 
            catch { }// (Exception e)
            {
                //code if u like 
            }
        }
    }
}
