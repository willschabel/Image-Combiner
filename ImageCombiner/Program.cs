using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace ImageCombiner
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Input the path containing chapters for your images: ");
            string path = Console.ReadLine();
            Console.WriteLine("\n");

            try
            {
                var Folders = Directory.EnumerateDirectories(@path);
                foreach(var childFolderPath in Folders)
                {
                    string imageOnePath = null;
                    string imageTwoPath = null;
                    Image imageOne = null;
                    Image imageTwo = null;
                    Bitmap bitmap = null;
                    int count = 0;
                    List<Bitmap> bitmaps = new List<Bitmap>();

                    string childPath = childFolderPath;
                    var imgFiles = Directory.EnumerateFiles(childPath, "*.jpg");

                    foreach (string currentImg in imgFiles)
                    {
                        count++;
                        Image img = Image.FromFile(currentImg);
                        if(imageOne == null)
                        {
                            imageOne = img;
                            imageOnePath = currentImg;
                        }
                        else
                        {
                            imageTwo = img;
                            imageTwoPath = currentImg;
                            bitmap = new Bitmap(Math.Max(imageOne.Width, imageTwo.Width), imageOne.Height + imageTwo.Height);
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                g.DrawImage(imageOne, new Rectangle(0, 0, Math.Max(imageOne.Width, imageTwo.Width), imageOne.Height));
                                g.DrawImage(imageTwo, new Rectangle(0, imageOne.Height - 1, Math.Max(imageOne.Width, imageTwo.Width), imageTwo.Height));
                            }

                            if(count == 4)
                            {
                                bitmaps.Add(bitmap);
                                imageOne.Dispose();
                                imageOne = null;
                                File.Delete(imageOnePath);
                                imageOnePath = null;
                                imageTwo.Dispose();
                                imageTwo = null;
                                File.Delete(@imageTwoPath);
                                imageTwoPath = null;

                                count = 0;
                            }
                            else
                            {
                                imageOne.Dispose();
                                imageOne = bitmap;

                                imageTwo.Dispose();
                                File.Delete(@imageTwoPath);
                                imageTwo = null;
                                imageTwoPath = null;
                            }
                        }
                    }

                    if(count != 0)
                    {
                        Bitmap overflow = new Bitmap(imageOne);
                        bitmaps.Add(overflow);
                        imageOne.Dispose();
                        File.Delete(@imageOnePath);

                        count = 0;
                    }
                    foreach(Bitmap b in bitmaps)
                    {
                        count++;
                        string newPath = childFolderPath + @"\" + count + ".jpg";
                        b.Save(@newPath, ImageFormat.Jpeg);
                        b.Dispose();
                    }
                    Console.WriteLine($"{childFolderPath} - Finished");
                }
                Console.WriteLine("\nAll of your images have been combined, press enter to exit.");
                Console.ReadLine();
            }

            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
