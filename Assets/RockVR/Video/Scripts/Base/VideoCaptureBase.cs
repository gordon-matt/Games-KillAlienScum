﻿using UnityEngine;
using RockVR.Common;

namespace RockVR.Video
{
    /// <summary>
    /// Base class for <c>VideoCapture</c> and <c>VideoCapturePro</c> class.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class VideoCaptureBase : MonoBehaviour
    {
        /// <summary>
        /// Capture mode.
        /// </summary>
        public enum ModeType
        {
            /// <summary>
            /// Save the video file locally.
            /// </summary>
            LOCAL,
            /// <summary>
            /// Live streaming video to rtmp server. 
            /// </summary>
            LIVE_STREAMING,
        }
        /// <summary>
        /// Capture video format type.
        /// </summary>
        public enum FormatType
        {
            /// <summary>
            /// Normal 2D video.
            /// </summary>
            NORMAL,
            /// <summary>
            /// Panorama video, for capture a 360 video.
            /// https://en.wikipedia.org/wiki/Panorama
            /// </summary>
            PANORAMA
        }
        /// <summary>
        /// Panorama projection type.
        /// </summary>
        public enum PanoramaProjectionType
        {
            /// <summary>
            /// Cubemap format.
            /// https://docs.unity3d.com/Manual/class-Cubemap.html
            /// </summary>
            /// Cubemap video format layout:
            /// +------------------+------------------+------------------+
            /// |                  |                  |                  |
            /// |                  |                  |                  |
            /// |    +X (Right)    |    -X (Left)     |     +Y (Top)     |
            /// |                  |                  |                  |
            /// |                  |                  |                  |
            /// +------------------+------------------+------------------+
            /// |                  |                  |                  |
            /// |                  |                  |                  |
            /// |   +Y (Bottom)    |   +Z (Fromt)     |    -Z (Back)     |
            /// |                  |                  |                  |
            /// |                  |                  |                  |
            /// +------------------+------------------+------------------+
            /// 
            CUBEMAP,
            /// <summary>
            /// Equirectangular format.
            /// https://en.wikipedia.org/wiki/Equirectangular_projection
            /// </summary>
            EQUIRECTANGULAR
        }
        /// <summary>
        /// Video stereo type.
        /// </summary>
        public enum StereoType
        {
            /// <summary>
            /// None stereo.
            /// </summary>
            NONE,
            /// <summary>
            /// Top-bottom stereo format.
            /// </summary>
            TOP_BOTTOM,
            /// <summary>
            /// Left-right stereo format.
            /// </summary>
            LEFT_RIGHT,
        }
        /// <summary>
        /// Frame size type.
        /// </summary>
        public enum FrameSizeType
        {
            /// <summary>
            /// 480p (640 x 480) Standard Definition (SD).
            /// </summary>
            _640x480,
            /// <summary>
            /// 480p (720 x 480) Standard Definition (SD) (resolution of DVD video).
            /// </summary>
            _720x480,
            /// <summary>
            /// 540p (960 x 540).
            /// </summary>
            _960x540,
            /// <summary>
            /// 720p (1280 x 720) High Definition (HD).
            /// </summary>
            _1280x720,
            /// <summary>
            /// 1080p (1920 x 1080) Full High Definition (FHD).
            /// </summary>
            _1920x1080,
            /// <summary>
            /// 2K (2048 x 1080).
            /// </summary>
            _2048x1080,
            /// <summary>
            /// 4K (3840 x 2160) Quad Full High Definition (QFHD)
            /// (also known as UHDTV/UHD-1, resolution of Ultra High Definition TV).
            /// </summary>
            _3840x2160,
            /// <summary>
            /// 4K (4096 x 2160) Ultra High Definition (UHD).
            /// </summary>
            _4096x2160,
            // Add your custom resolution here (modify frameWidth, frameHeight accordingly):
        }
        /// <summary>
        /// Cubemap size type.
        /// </summary>
        public enum CubemapSizeType
        {
            _512,
            _1024,
            _2048,
        }
        /// <summary>
        /// Encode quality type.
        /// </summary>
        public enum EncodeQualityType
        {
            /// <summary>
            /// Lower quality will decrease filesize on disk.
            /// Low = 1000 bitrate.
            /// </summary>
            Low,
            /// <summary>
            /// Medium = 2500 bitrate.
            /// </summary>
            Medium,
            /// <summary>
            /// High = 5000 bitrate.
            /// </summary>
            High,
        }
        /// <summary>
        /// Anti aliasing type.
        /// </summary>
        public enum AntiAliasingType
        {
            _1,
            _2,
            _4,
            _8,
        }
        /// <summary>
        /// Target framerate type.
        /// </summary>
        public enum TargetFramerateType
        {
            _18,
            _24,
            _30,
            _45,
            _60,
        }
        /// <summary>
        /// The type of captured video format.
        /// </summary>
        [Tooltip("Decide to capture normal or panorama video")]
        public FormatType format = FormatType.NORMAL;
        /// <summary>
        /// The type of captured video stereo format.
        /// </summary>
        [Tooltip("Decide to capture stereo video")]
        public StereoType stereoFormat = StereoType.NONE;
        /// <summary>
        /// The size of the frame.
        /// </summary>
        [Tooltip("Resolution of captured video")]
        public FrameSizeType frameSize = FrameSizeType._1280x720;
        /// <summary>
        /// The size of the cubemap.
        /// </summary>
        [Tooltip("The cubemap size capture render to")]
        public CubemapSizeType _cubemapSize = CubemapSizeType._1024;
        /// <summary>
        /// The type of the projection.
        /// </summary>
        [Tooltip("The panorama projection type")]
        public PanoramaProjectionType panoramaProjection = PanoramaProjectionType.CUBEMAP;
        /// <summary>
        /// The encode quality setting.
        /// </summary>
        [Tooltip("Lower quality will decrease file size on disk")]
        public EncodeQualityType encodeQuality = EncodeQualityType.Medium;
        /// <summary>
        /// The anti aliasing setting.
        /// </summary>
        [Tooltip("Anti aliasing setting for captured video")]
        public AntiAliasingType _antiAliasing = AntiAliasingType._1;
        /// <summary>
        /// The target framerate setting.
        /// </summary>
        [Tooltip("Target frame rate for captured video")]
        public TargetFramerateType _targetFramerate = TargetFramerateType._30;
        /// <summary>
        /// Get the width of the video frame.
        /// </summary>
        public int frameWidth
        {
            get
            {
                if (!isDedicated)
                {
                    // If frame size odd number, encode will stuck.
                    if (captureCamera.pixelWidth % 2 == 0)
                    {
                        return captureCamera.pixelWidth;
                    }
                    else
                    {
                        return captureCamera.pixelWidth - 1;
                    }
                }
                if (format == FormatType.PANORAMA)
                {
                    if (panoramaProjection == PanoramaProjectionType.CUBEMAP)
                    {
                        return cubemapSize * 3;
                    }
                }
                if (frameSize == FrameSizeType._640x480) { return 640; }
                if (frameSize == FrameSizeType._720x480) { return 720; }
                if (frameSize == FrameSizeType._960x540) { return 960; }
                if (frameSize == FrameSizeType._1280x720) { return 1280; }
                if (frameSize == FrameSizeType._1920x1080) { return 1920; }
                if (frameSize == FrameSizeType._2048x1080) { return 2048; }
                if (frameSize == FrameSizeType._3840x2160) { return 3840; }
                if (frameSize == FrameSizeType._4096x2160) { return 4096; }
                return 0;
            }
        }
        /// <summary>
        /// Get the height of the video frame.
        /// </summary>
        public int frameHeight
        {
            get
            {
                if (!isDedicated)
                {
                    // If frame size odd number, encode will stuck.
                    if (captureCamera.pixelHeight % 2 == 0)
                    {
                        return captureCamera.pixelHeight;
                    }
                    else
                    {
                        return captureCamera.pixelHeight - 1;
                    }
                }
                if (format == FormatType.PANORAMA)
                {
                    if (panoramaProjection == PanoramaProjectionType.CUBEMAP)
                    {
                        return cubemapSize * 2;
                    }
                }
                if (frameSize == FrameSizeType._640x480 ||
                    frameSize == FrameSizeType._720x480) { return 480; }
                if (frameSize == FrameSizeType._960x540) { return 540; }
                if (frameSize == FrameSizeType._1280x720) { return 720; }
                if (frameSize == FrameSizeType._1920x1080 ||
                    frameSize == FrameSizeType._2048x1080) { return 1080; }
                if (frameSize == FrameSizeType._3840x2160 ||
                    frameSize == FrameSizeType._4096x2160) { return 2160; }
                return 0;
            }
        }
        /// <summary>
        /// Get the cubemap size value.
        /// </summary>
        public int cubemapSize
        {
            get
            {
                if (_cubemapSize == CubemapSizeType._512) { return 512; }
                if (_cubemapSize == CubemapSizeType._1024) { return 1024; }
                if (_cubemapSize == CubemapSizeType._2048) { return 2048; }
                return 0;
            }
        }
        /// <summary>
        /// Get the anti-aliasing value.
        /// </summary>
        public int antiAliasing
        {
            get
            {
                if (_antiAliasing == AntiAliasingType._1) { return 1; }
                if (_antiAliasing == AntiAliasingType._2) { return 2; }
                if (_antiAliasing == AntiAliasingType._4) { return 4; }
                if (_antiAliasing == AntiAliasingType._8) { return 8; }
                return 0;
            }
        }
        /// <summary>
        /// Get the bitrate value.
        /// </summary>
        public int bitrate
        {
            get
            {
                if (encodeQuality == EncodeQualityType.Low) { return 1000; }
                if (encodeQuality == EncodeQualityType.Medium) { return 2500; }
                if (encodeQuality == EncodeQualityType.High) { return 5000; }
                return 0;
            }
        }

        /// <summary>
        /// Get the target framerate value.
        /// </summary>
        public int targetFramerate
        {
            get
            {
                if (_targetFramerate == TargetFramerateType._18) { return 18; }
                if (_targetFramerate == TargetFramerateType._24) { return 24; }
                if (_targetFramerate == TargetFramerateType._30) { return 30; }
                if (_targetFramerate == TargetFramerateType._45) { return 45; }
                if (_targetFramerate == TargetFramerateType._60) { return 60; }
                return 0;
            }
        }
        /// <summary>
        /// The camera that resides on the same game object as this script.
        /// It's render content will be used for capturing video.
        /// </summary>
        /// <value>The camera.</value>
        protected Camera captureCamera;
        /// <summary>
        /// Specifies whether or not the camera being used to capture video is 
        /// dedicated solely to video capture. When a dedicated camera is used,
        /// the camera's aspect ratio will automatically be set to the specified
        /// frame size.
        /// If a non-dedicated camera is specified it is assumed the camera will 
        /// also be used to render to the screen, and so the camera's aspect 
        /// ratio will not be adjusted.
        /// Use a dedicated camera to capture video at resolutions that have a 
        /// different aspect ratio than the device screen.
        /// </summary>
        public bool isDedicated = true;
        /// <summary>
        /// The delta time of each frame.
        /// </summary>
        protected float deltaFrameTime;
        /// <summary>
        /// If set live streaming mode, encoded video will be push to remote
        /// rtmp server instead of save to local file.
        /// </summary>
        public ModeType mode = ModeType.LOCAL;
        /// <summary>
        /// The captured camera video path.
        /// </summary>
        public string filePath { get; protected set; }
        /// <summary>
        /// Live stream sever address.
        /// </summary>
        public string streamingAddress;
        /// <summary>
        /// Delegate to register event.
        /// </summary>
        public EventDelegate eventDelegate;
        /// <summary>
        /// The distance between the stereoscopic camera’s two viewpoints.
        /// </summary>
        public float interPupillaryDistance = 0.0635f; // Average IPD of all subjects in US Army survey in meters.        
        /// <summary>
        /// The material for processing stereoscopic video format.
        /// </summary>
        public Material stereoPackMaterial;
        /// <summary>
        /// The material for copy stereo video.
        /// </summary>
        protected Material blitMaterial;
        /// <summary>
        /// The stereo target texture.
        /// </summary>
        protected RenderTexture stereoTargetTexture;
        /// <summary>
        /// The final stereo target.
        /// </summary>
        protected RenderTexture finalTargetTexture;
        /// <summary>
        /// Initialize video capture parameters.
        /// </summary>
        protected void Awake()
        {
            captureCamera = GetComponent<Camera>();
            deltaFrameTime = 1f / targetFramerate;
            eventDelegate = new EventDelegate();
        }
        /// <summary>
        /// Start capture video.
        /// </summary>
        public virtual void StartCapture() { }
        /// <summary>
        /// Stop capture video.
        /// </summary>
        public virtual void StopCapture() { }
        /// <summary>
        /// Check if is capture panorama video.
        /// </summary>
        public bool isPanorama { get { return format == FormatType.PANORAMA; } }
        /// <summary>
        /// Check if is live streaming mode.
        /// </summary>
        public bool isLiveStreaming { get { return mode == ModeType.LIVE_STREAMING; } }
        /// <summary>
        /// Save render texture to PNG image file.
        /// </summary>
        /// <param name="rtex">Rtex.</param>
        /// <param name="fileName">File name.</param>
        protected void RenderTextureToPNG(RenderTexture rtex, string fileName)
        {
            Texture2D tex = new Texture2D(rtex.width, rtex.height, TextureFormat.RGB24, false);
            RenderTexture.active = rtex;
            tex.ReadPixels(new Rect(0, 0, rtex.width, rtex.height), 0, 0, false);
            RenderTexture.active = null;
            TextureToPNG(tex, fileName);
        }
        /// <summary>
        /// Save texture to PNG image file.
        /// </summary>
        /// <param name="tex">Tex.</param>
        /// <param name="fileName">File name.</param>
        protected void TextureToPNG(Texture2D tex, string fileName)
        {
            string filePath = PathConfig.saveFolder + fileName;
            byte[] imageBytes = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, imageBytes);
            Debug.Log("[VideoCaptureBase::TextureToPNG] Save " + filePath);
        }
        /// <summary>
        /// Conversion video format to stereo.
        /// </summary>
        protected void SetStereoVideoFormat(RenderTexture frameRenderTexture)
        {
            Vector3 cameraPosition = captureCamera.transform.position;
            // Left eye
            captureCamera.transform.Translate(new Vector3(-interPupillaryDistance, 0, 0), Space.Self);
            RenderCameraToRenderTexture(captureCamera, frameRenderTexture, stereoTargetTexture);
            if (stereoFormat == StereoType.TOP_BOTTOM)
            {
                stereoPackMaterial.DisableKeyword("STEREOPACK_BOTTOM");
                stereoPackMaterial.EnableKeyword("STEREOPACK_TOP");
            }
            else if (stereoFormat == StereoType.LEFT_RIGHT)
            {
                stereoPackMaterial.DisableKeyword("STEREOPACK_RIGHT");
                stereoPackMaterial.EnableKeyword("STEREOPACK_LEFT");
            }
            Graphics.Blit(stereoTargetTexture, finalTargetTexture, stereoPackMaterial);
            // Right eye
            captureCamera.transform.localPosition = cameraPosition;
            captureCamera.transform.Translate(new Vector3(interPupillaryDistance, 0f, 0f), Space.Self);
            RenderCameraToRenderTexture(captureCamera, frameRenderTexture, stereoTargetTexture);
            if (stereoFormat == StereoType.TOP_BOTTOM)
            {
                stereoPackMaterial.EnableKeyword("STEREOPACK_BOTTOM");
                stereoPackMaterial.DisableKeyword("STEREOPACK_TOP");
            }
            else if (stereoFormat == StereoType.LEFT_RIGHT)
            {
                stereoPackMaterial.EnableKeyword("STEREOPACK_RIGHT");
                stereoPackMaterial.DisableKeyword("STEREOPACK_LEFT");
            }
            Graphics.Blit(stereoTargetTexture, finalTargetTexture, stereoPackMaterial);
            // Restore camera state
            captureCamera.transform.localPosition = cameraPosition;
        }
        /// <summary>
        /// Get render data from camera to render texture.
        /// </summary>
        /// <param name="camera">The render camera</param>
        /// <param name="stereoTarget">The stereo target</param>
        private void RenderCameraToRenderTexture(Camera camera, RenderTexture frameRenderTexture, RenderTexture stereoTarget)
        {
            camera.CopyFrom(captureCamera);
            if (isDedicated)
            {
                camera.targetTexture = frameRenderTexture;
            }
            camera.Render();
            Graphics.SetRenderTarget(stereoTarget);
            Graphics.Blit(frameRenderTexture, blitMaterial);
            Graphics.SetRenderTarget(null);
        }
    }
}