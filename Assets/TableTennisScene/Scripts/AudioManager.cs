using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Diagnostics;
using debug = UnityEngine.Debug;

namespace TT
{
    public class AudioManager : MonoBehaviour
    {
        public InputHelpers.Button audiobutton = InputHelpers.Button.SecondaryButton;
        public InputHelpers.Button Stopbutton = InputHelpers.Button.PrimaryButton;
        public AudioSource bgmPlayer;
        public Dictionary<int, long> notes;
        //몇 "ms"에 Kick(물체 날라오는지) 에 대한 정보 (note index, time)의 구조로 파일 할당
        private Pitcher pitcher;
        private CollisionManager collisionManager;
        void Update()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            InputDevice stopdevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            bool start = false;
            Stopwatch sw = new Stopwatch();
            int noteindex = 1;
            long checktime = 0;
            if (device.isValid)
            {    
                InputHelpers.IsPressed(device, audiobutton, out start);
            }
            while (start)
            {
                bgmPlayer.Play();
                sw.Start();
                if (sw.ElapsedMilliseconds == notes[noteindex])
                {
                    noteindex++;
                    //공이 날라가는 시간 2초 => 2초 후 이벤트 감지
                    pitcher.ThrowBall(2);
                    checktime = sw.ElapsedMilliseconds;

                }
                if (sw.ElapsedMilliseconds - checktime <= 2200 && collisionManager.checking()) // for every 0.2ms
                {
                    // 점수 및 판정 todo
                }
                else if (sw.ElapsedMilliseconds - checktime <= 2500 && collisionManager.checking()) // for ever 0.5ms
                {
                    // 점수 및 판정 todo 
                }
                if (device.isValid)
                {   
                    //곡 종료 및 판정 종료
                    InputHelpers.IsPressed(stopdevice, Stopbutton, out start);
                }
            }

        }
    }
}