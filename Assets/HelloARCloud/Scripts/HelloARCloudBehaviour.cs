//=============================================================================================================================
//
// Copyright (c) 2015-2018 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using EasyAR;

namespace Sample
{
    public class HelloARCloudBehaviour : CloudRecognizerBehaviour
    {
        private List<string> uids = new List<string>();
        private ImageTrackerBaseBehaviour trackerBehaviour;
        private string persistentDataPath;
        public bool SaveNewTarget;

        private void Awake()
        {
            if (Server.Length <= 0 || Key.Length <= 0 || Secret.Length <= 0)
                Debug.LogError("Cloud not setup! Setup 'CloudRecognizer' in the scene or setup values in the scripts.");

            FindObjectOfType<EasyARBehaviour>().Initialize();
            persistentDataPath = Application.persistentDataPath;
            CloudUpdate += OnCloudUpdate;
            WorkStart += OnWorkStart;
            if (ARBuilder.Instance.ImageTrackerBehaviours.Count > 0)
                trackerBehaviour = ARBuilder.Instance.ImageTrackerBehaviours[0];
        }

        private void OnWorkStart(DeviceUserAbstractBehaviour cloud, DeviceAbstractBehaviour camera)
        {
            Debug.Log("cloud start to work!");
        }

        private void OnCloudUpdate(CloudRecognizerBaseBehaviour cloud, Status status, List<ImageTarget> targets)
        {
            if (status != Status.Success && status != Status.Fail)
            {
                Debug.LogWarning("cloud: " + status);
            }
            if (!trackerBehaviour)
                return;
            foreach (var imageTarget in targets)
            {
                if(uids.Contains(imageTarget.Uid))
                    continue;

                Debug.Log("New Cloud Target: " + imageTarget.Uid + "(" + imageTarget.Name + ")");
                uids.Add(imageTarget.Uid);
                var gameObj = new GameObject();
                var targetBehaviour = gameObj.AddComponent<SampleImageTargetBehaviour>();
                if (!targetBehaviour.SetupWithTarget(imageTarget))
                    continue;
                targetBehaviour.Bind(trackerBehaviour);

                var gameObj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObj2.GetComponent<MeshRenderer>().material = Resources.Load("Materials/EasyAR") as Material;
                gameObj2.transform.parent = gameObj.transform;
                gameObj2.transform.localPosition = Vector3.up/10;
                gameObj2.transform.localScale = new Vector3(targetBehaviour.Size.x / Mathf.Max(targetBehaviour.Size.x, targetBehaviour.Size.y)/2, 1f/5, targetBehaviour.Size.y / Mathf.Max(targetBehaviour.Size.x, targetBehaviour.Size.y)/2);

                if (SaveNewTarget)
                {
                    var thread = new Thread(SaveRunner) { Priority = System.Threading.ThreadPriority.BelowNormal };
                    thread.Start(imageTarget);
                }
            }
        }

        private void SaveRunner(object args)
        {
            var imageTarget = args as ImageTarget;
            var image = imageTarget.Images[0];

            byte[] fileHeader = new byte[14] { (byte)'B', (byte)'M', 0, 0, 0, 0, 0, 0, 0, 0, 54, 4, 0, 0 };
            byte[] infoHeader = new byte[40] { 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] pallate = new byte[1024];
            byte[] bmpPad = new byte[3] { 0, 0, 0 };

            int fileSize = 54 + 1024 + image.Width * image.Height;
            fileHeader[2] = (byte)(fileSize);
            fileHeader[3] = (byte)(fileSize >> 8);
            fileHeader[4] = (byte)(fileSize >> 16);
            fileHeader[5] = (byte)(fileSize >> 24);

            infoHeader[4] = (byte)(image.Width);
            infoHeader[5] = (byte)(image.Width >> 8);
            infoHeader[6] = (byte)(image.Width >> 16);
            infoHeader[7] = (byte)(image.Width >> 24);
            infoHeader[8] = (byte)(image.Height);
            infoHeader[9] = (byte)(image.Height >> 8);
            infoHeader[10] = (byte)(image.Height >> 16);
            infoHeader[11] = (byte)(image.Height >> 24);
            for (int i = 0; i < 256; i++)
            {
                pallate[4 * i] = (byte)i;
                pallate[4 * i + 1] = (byte)i;
                pallate[4 * i + 2] = (byte)i;
                pallate[4 * i + 3] = 0;
            }

            FileStream fs = File.Create(Path.Combine(persistentDataPath, imageTarget.Uid + ".bmp"));
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(fileHeader);
            bw.Write(infoHeader);
            bw.Write(pallate);

            for (int i = 0; i < image.Height; i++)
            {
                bw.Write(image.Pixels, image.Width * (image.Height - i - 1), image.Width);
                if (image.Width % 4 > 0)
                {
                    bw.Write(bmpPad, 0, 4 - image.Width % 4);
                }
            }

            bw.Close();
            fs.Close();

            string json = ""
                          + @"{"
                          + @"  ""images"": [{"
                          + @"    ""image"" : " + @"""" + imageTarget.Uid + ".bmp" + @"""" + ","
                          + @"    ""name"" : " + @"""" + imageTarget.Name + @"""" + ","
                          + @"    ""size"" : " + "[" + imageTarget.Size.x + ", " + imageTarget.Size.y + "]" + ","
                          + @"    ""uid"" : " + @"""" + imageTarget.Uid + @"""" + ","
                          + @"    ""meta"" : " + @"""" + imageTarget.MetaData + @"""" + "  }]"
                          + @"}";
            File.WriteAllText(Path.Combine(persistentDataPath, imageTarget.Uid + ".json"), json);
            Debug.Log("saved: " + imageTarget.Uid + " -> " + persistentDataPath);
        }
    }
}
