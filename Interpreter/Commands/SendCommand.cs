﻿/**************************************************************************
 *                                                                        *
 *  File:        SendCommand.cs                                           *
 *  Copyright:   (c) Olăreț Radu                                          *
 *               @Kakerou_CLUB                                            *
 *  Description: Command that sends raw text as keystrokes to the event   *
 *               queue                                                    *
 **************************************************************************/

using InputListener;
using System.Runtime.InteropServices;

namespace Interpreter
{
    /// <summary>
    /// Command that sends raw text as keystrokes to the event queue of the OS
    /// </summary>
    internal class SendCommand : IKlppCommand
    {
        private string _text;

        public SendCommand(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Sends raw text as keystrokes to the event queue of the OS
        /// </summary>
        public void Execute()
        {
            INPUT[] inputArray = new INPUT[_text.Length * 2];
            for(int i = 0; i < _text.Length; i++)
            {
                inputArray[2 * i] = new INPUT
                {
                    type = InputType.INPUT_KEYBOARD,
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = (short)_text[i],
                        dwFlags = KeyEvent.KEYEVENTF_UNICODE 
                    }
                } ;
                inputArray[2 * i + i] = new INPUT
                {
                    type = InputType.INPUT_KEYBOARD,
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = (short)_text[i],
                        dwFlags = KeyEvent.KEYEVENTF_UNICODE | KeyEvent.KETEVENTF_KEYUP
                    }
                };
            }
            LLInput.SendInput((uint)inputArray.Length, inputArray, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}
