using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        //写入数据流到文件
        public static void SaveStreamToFile(Stream FromStream, string TargetFile)
        {
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
