using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System.Threading;

namespace ICSharpCode.SharpZipLib
{
    public delegate void UnZipCompletedHandler(string name, SharpZipResult result);

    public class SharpZipResult
    {
        public ZipStatus zs;
        public string name;
        public string outPath;
        public string message;
        public float progress;
    }

    public enum ZipStatus
    {
        ZS_OK       = 0,    // 0
        ZS_PROGRESS,        // 1
        ZS_ERROR,           // 2
        ZS_CANCEL,          // 3
        ZS_INVALID,         // 4
    }

    public class ZipFileEntry
    {
        public string name;
        public string fullPath;
        public int type;    // type
        public string srcPath;
        public string outPath;
        public  UnZipCompletedHandler handler;
    }

    public  class SharpZipToolkit
    {
        // zip a folder to zip file
        public  static  void    ZipFolder(
            string  path,
            string  outFilename
        )
        {
            // Perform some simple parameter checking.  More could be done
            // like checking the target file name is ok, disk space, and lots
            // of other things, but for a demo this covers some obvious traps.
            if (!Directory.Exists(path))
            {
                Debug.Log("Cannot find directory path");
                return;
            }

            try
            {
                // Depending on the directory this could be very large and would require more attention
                // in a commercial package.
                string[] filenames = Directory.GetFiles(path);

                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using (ZipOutputStream s = new ZipOutputStream(File.Create(outFilename)))
                {

                    s.SetLevel(9); // 0 - store only to 9 - means best compression

                    byte[] buffer = new byte[4096];

                    foreach (string file in filenames)
                    {

                        // Using GetFileName makes the result compatible with XP
                        // as the resulting path is not absolute.
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(file))
                        {

                            // Using a fixed size buffer here makes no noticeable difference for output
                            // but keeps a lid on memory usage.
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }

                    // Finish/Close arent needed strictly as the using statement does this automatically

                    // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                    // the created file would be invalid.
                    s.Finish();

                    // Close is important to wrap things up and unlock the file.
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception during processing " + ex.Message);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }   // end of ZipFolder

        // unzip 
        public  static  void    UnZipToFolder(
            string  zipFilename,
            string  outPath
        )
        {
            // Perform simple parameter checking.
            if (!File.Exists(zipFilename))
            {
                Debug.Log("Cannot find file zipFilename");
                return;
            }

            // check out path
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilename)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                   // Debug.Log(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(outPath + "/" + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
						string outputName = outPath + "/" + theEntry.Name;
						if(File.Exists(outputName))
							continue;

						using (FileStream streamWriter = File.Create(outputName))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

        }   // end of unzip 

        // unzip 
        public static void UnZipEntryToFile(
            string zipFilename,
            string  zipEntryName,
            string outPath
        )
        {
            // Perform simple parameter checking.
            if (!File.Exists(zipFilename))
            {
                Debug.Log("Cannot find file zipFilename");
                return;
            }

            // check out path
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilename)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    Debug.Log("DEBUG:UnZipEntryToFile, entry name = " + theEntry.Name);
                    Debug.Log("DEBUG:UnZipEntryToFile, zipEntryName = " + zipEntryName);
                    if (!zipEntryName.Equals(theEntry.Name))
                    {
                        continue;
                    }

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(outPath + "/" + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(outPath + "/" + fileName))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    Debug.Log("DEBUG:UnZipEntryToFile, unzip success = " + outPath + "/" + fileName);
                    break;
                }
            }
        }   // end of unzip entry to file

		// compress bytes
		public static byte[] CompressBytes(byte[] input)  
		{  
			// Create the compressor with highest level of compression  
			Deflater compressor = new Deflater();  
			compressor.SetLevel(Deflater.BEST_COMPRESSION);  
			
			// Give the compressor the data to compress  
			compressor.SetInput(input);  
			compressor.Finish();  
			
			/* 
             * Create an expandable byte array to hold the compressed data. 
             * You cannot use an array that's the same size as the orginal because 
             * there is no guarantee that the compressed data will be smaller than 
             * the uncompressed data. 
             */  
			MemoryStream bos = new MemoryStream(input.Length);  
			
			// Compress the data  
			byte[] buf = new byte[1024];  
			while (!compressor.IsFinished)  
			{  
				int count = compressor.Deflate(buf);  
				bos.Write(buf, 0, count);  
			}  
			
			// Get the compressed data  
			return bos.ToArray();  
		}  // end of compress bytes

		// uncompress bytes
		public static byte[] UncompressBytes(byte[] input)  
		{  
			Inflater decompressor = new Inflater();  
			decompressor.SetInput(input);  
			
			// Create an expandable byte array to hold the decompressed data  
			MemoryStream bos = new MemoryStream(input.Length);  
			
			// Decompress the data  
			byte[] buf = new byte[1024];  
			while (!decompressor.IsFinished)  
			{  
				int count = decompressor.Inflate(buf);  
				bos.Write(buf, 0, count);  
			}  
			
			// Get the decompressed data  
			return bos.ToArray();  
		}  // end of uncompress bytes


        //////////////////////////////////
        // async operation

        // unzip 
        public static void UnZipToFolder(
            string zipFilename,
            string outPath,
            UnZipCompletedHandler   handler
        )
        {
            // Perform simple parameter checking.
            if (!File.Exists(zipFilename))
            {
                SharpZipResult r = new SharpZipResult();
                r.zs = ZipStatus.ZS_ERROR;
                r.name = zipFilename;
                r.outPath = outPath;
                r.message = "Cannot find file zipFilename";
                if (handler != null)
                {
                    handler(zipFilename, r);
                }
				UnityEngine.Debug.LogError ("the pck file of zipFilename : "  + zipFilename + ", not exist!");
                return;
            }

            // check out path
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

			DateTime startTime = DateTime.Now;
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilename)))
            {
                if (s == null)
                {
                    SharpZipResult r = new SharpZipResult();
                    r.name = zipFilename;
                    r.outPath = outPath;
                    r.zs = ZipStatus.ZS_ERROR;
                    r.message = "Cannot open file zipFilename";
                    if (handler != null)
                    {
                        handler(zipFilename, r);
                    }
                    return;
                }

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    //Debug.Log(theEntry.Name);
				
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(outPath + "/" + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(outPath + "/" + theEntry.Name))
                        {
                            int readSize = 0;
							bool	canProgress = false;
                            float len = 1.0f;
							if (theEntry.Size >= 0)
							{
								len = (float)s.Length;
								canProgress = true;
							}
							else
								Debug.Log("the entiry size is : " +theEntry.Size + "-1 will not show progress"); 

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                                // total read size
                                readSize += size;

                                // progress handler
                                SharpZipResult r = new SharpZipResult();
                                r.zs = ZipStatus.ZS_PROGRESS;
                                r.name = theEntry.Name;
                                r.outPath = outPath;

								if (canProgress)
                                	r.progress = readSize / len;

                               // Horizon.Debug.Log("DEBUG:UnZipToFolder, name = " + theEntry.Name + " readSize = " + readSize + " s.length = " + s.Length);
								//UnityEngine.Debug.LogError(theEntry.Size +", "+r.progress + ", " + handler);
								if (canProgress && handler != null)
                                {
                                    handler(zipFilename, r);
                                }

                            }   // end of while
                            streamWriter.Close();
                        }   // end of streamWriter

                    }
                }
            }

            SharpZipResult zr = new SharpZipResult();
            zr.zs = ZipStatus.ZS_OK;
            zr.name = zipFilename;
            zr.outPath = outPath;
            if (handler != null)
            {
                handler(zipFilename, zr);
            }

			TimeSpan span = DateTime.Now - startTime;
			UnityEngine.Debug.Log ("SharpZipToolkit------> fileName=" + zipFilename + ", Span.TotalSeconds :" + span.TotalSeconds);

        }   // end of unzip 

        // unzip 
        public  static  void    UnZipToFolder(
            System.Object    obj
        )
        {
            ZipFileEntry entry = obj as ZipFileEntry;

            try
            {
                UnZipToFolder(entry.fullPath, entry.outPath, entry.handler);
            }
            catch (System.Exception ex)
            {
            	// error handle
                SharpZipResult r = new SharpZipResult();
                r.zs = ZipStatus.ZS_ERROR;
                r.name = entry.fullPath;
                r.outPath = entry.outPath;
                r.message = ex.Message;
                entry.handler(entry.fullPath, r);
            }
        }

        // unzip async
        public  static  void    UnZipToFolderAsync(
            string  zipFilename,
            string  outPath,
            UnZipCompletedHandler   handler
        )
        {
            // start a new thread
            Thread t = new Thread(new ParameterizedThreadStart(UnZipToFolder));
            ZipFileEntry entry = new ZipFileEntry();
            entry.fullPath = zipFilename;
            entry.outPath = outPath;
            entry.handler = handler;
            t.Start(entry);
        }
    }
}
