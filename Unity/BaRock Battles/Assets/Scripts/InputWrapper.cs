using UnityEngine;
using System.Collections;


namespace InputWrapper
{
    public enum EKeyMode
    {
        EKeyMode_SingleInputDevice,
        EKeyMode_DualInputDevicePrioritizeKeyboard,
        EKeyMode_DualInputDevicePrioritizeController,
    }

    //======================
    // Enumarations
    //======================
    public enum EKeyId
    {
        // In-Game
        EKeyId_Action1,
        EKeyId_Action2,

        // Menu navigation
        EKeyId_Confirm,
        EKeyId_Cancel,
        EKeyId_Start,
    }
    public enum EKeyPairId
    {
        EKeyPairId_HorizontalRight,
        EKeyPairId_VerticalRight,
        EKeyPairId_HorizontalLeft,
        EKeyPairId_VerticalLeft,
    }
    public enum EKeyPairHalf
    {
        EKeyPairHalf_Neg,
        EKeyPairHalf_Pos,
    }
    public enum EButton
    {
        EButton_A           = 0,
        EButton_B           = 1,
        EButton_X           = 2,
        EButton_Y           = 3,
        EButton_LeftBumper  = 4,
        EButton_RightBumper = 5,
        EButton_Back        = 6,
        EButton_Start       = 7,
        EButton_LeftJoy     = 8,
        EButton_RightJoy    = 9,

        EButton_None = int.MaxValue,
    }
    public enum EAxis
    {
        EAxis_LX       = 1,
        EAxis_LY       = 2,
        EAxis_RX       = 3,
        EAxis_RY       = 4,

        EAxis_None = int.MaxValue,
    }

    //======================
    // Structures
    //======================
    [System.Serializable]
    public class Key
    {
        public enum EState
        {
            EState_KeyboardButton,
            EState_JoystickButton,
        }
        public EState  state;
        public KeyCode key    = KeyCode.None;
        public EButton button = EButton.EButton_None;
    };
    [System.Serializable]
    public class KeyPair
    {
        public enum EState
        {
            EState_KeyboardButtonPair,
            EState_JoystickButtonPair,
            EState_JoystickAxis,
        }
        public EState  state;
        public KeyCode keyNeg    = KeyCode.None;
        public KeyCode keyPos    = KeyCode.None;
        public EButton buttonNeg = EButton.EButton_None;
        public EButton buttonPos = EButton.EButton_None;
        public EAxis   axis      = EAxis.EAxis_None;
    };

    //======================
    // Scheme
    //======================
    [System.Serializable]
    public class Scheme
    {
        public Scheme(int a_DeviceId)
        {
            m_DeviceId = a_DeviceId + 1;
            
            m_Action1    = new Key();
            m_Action2    = new Key();

            m_Confirm    = new Key();
            m_Cancel     = new Key();
            m_Start      = new Key();
            
            m_HorizontalLeft = new KeyPair();
            m_VerticalRight  = new KeyPair();

            m_HorizontalLeft = new KeyPair();
            m_VerticalRight  = new KeyPair();
        }
        public void CopyMapping(Scheme a_Scheme)
        {
            m_Action1    = a_Scheme.m_Action1;
            m_Action2    = a_Scheme.m_Action2;

            m_Confirm    = a_Scheme.m_Confirm;
            m_Cancel     = a_Scheme.m_Cancel;
            m_Start      = a_Scheme.m_Start;

            m_HorizontalLeft = a_Scheme.m_HorizontalLeft;
            m_VerticalLeft   = a_Scheme.m_VerticalLeft;

            m_HorizontalRight = a_Scheme.m_HorizontalRight;
            m_VerticalRight = a_Scheme.m_VerticalRight;
        }

        int m_DeviceId;

        //======================
        // Events
        //======================
        public bool GetDown(EKeyId a_Id)
        {
            Key k = GetKey(a_Id);
            
            switch(Defines.KEY_MODE)
            {
                case EKeyMode.EKeyMode_SingleInputDevice:
                    switch (k.state)
                    {
                        case Key.EState.EState_KeyboardButton:
                            return Input.GetKeyDown(k.key);
                        case Key.EState.EState_JoystickButton:
                            return k.button == EButton.EButton_None ? false : Input.GetKeyDown("joystick " + m_DeviceId + " button " + (int)k.button);
                    }
                    return false;
                case EKeyMode.EKeyMode_DualInputDevicePrioritizeKeyboard:
                    {
                        if (Input.GetKeyDown(k.key)) return true;
                        if (k.button != EButton.EButton_None && Input.GetKeyDown("joystick " + m_DeviceId + " button " + (int)k.button)) return true;
                    }
                    return false;
                case EKeyMode.EKeyMode_DualInputDevicePrioritizeController:
                    {
                        if (k.button != EButton.EButton_None && Input.GetKeyDown("joystick " + m_DeviceId + " button " + (int)k.button)) return true;
                        if (Input.GetKeyDown(k.key)) return true;
                    }
                    return false;
            }
            return false;
        }
        public bool GetHold(EKeyId a_Id)
        {
            Key k = GetKey(a_Id);

            switch (Defines.KEY_MODE)
            {
                case EKeyMode.EKeyMode_SingleInputDevice:
                    switch (k.state)
                    {
                        case Key.EState.EState_KeyboardButton:
                            return Input.GetKey(k.key);
                        case Key.EState.EState_JoystickButton:
                            return k.button == EButton.EButton_None ? false : Input.GetKey("joystick " + m_DeviceId + " button " + (int)k.button);
                    }
                    return false;
                case EKeyMode.EKeyMode_DualInputDevicePrioritizeKeyboard:
                    {
                        if (Input.GetKey(k.key)) return true;
                        if (k.button != EButton.EButton_None && Input.GetKey("joystick " + m_DeviceId + " button " + (int)k.button)) return true;
                    }
                    return false;
                case EKeyMode.EKeyMode_DualInputDevicePrioritizeController:
                    {
                        if (k.button != EButton.EButton_None && Input.GetKey("joystick " + m_DeviceId + " button " + (int)k.button)) return true;
                        if (Input.GetKey(k.key)) return true;
                    }
                    return false;
            }
            return false;
        }
        public float GetPress(EKeyPairId a_Id)
        {
            KeyPair kp = GetKeyPair(a_Id);

            switch (Defines.KEY_MODE)
            {
                case EKeyMode.EKeyMode_SingleInputDevice:
                    switch (kp.state)
                    {
                        case KeyPair.EState.EState_KeyboardButtonPair:
                            {
                                float mag = 0.0f;
                                if (Input.GetKey(kp.keyNeg)) mag -= 1.0f;
                                if (Input.GetKey(kp.keyPos)) mag += 1.0f;
                                return mag;
                            }
                        case KeyPair.EState.EState_JoystickButtonPair:
                            {
                                float mag = 0.0f;
                                string jStr = "joystick " + m_DeviceId + " button ";
                                if (kp.buttonNeg != EButton.EButton_None && Input.GetKey(jStr + (int)kp.buttonNeg)) mag -= 1.0f;
                                if (kp.buttonPos != EButton.EButton_None && Input.GetKey(jStr + (int)kp.buttonPos)) mag += 1.0f;
                                return mag;
                            }
                        case KeyPair.EState.EState_JoystickAxis:
                            return Input.GetAxis("joystick " + m_DeviceId + " axis " + (int)kp.axis);
                    }
                    return 0.0f;
                case EKeyMode.EKeyMode_DualInputDevicePrioritizeKeyboard:
                    {
                        float mag = 0.0f;
                        
                        // Keyboard
                        if (Input.GetKey(kp.keyNeg)) mag -= 1.0f;
                        if (Input.GetKey(kp.keyPos)) mag += 1.0f;
                        // Controller - buttons
                        string jStr = "joystick " + m_DeviceId + " button ";
                        if (kp.buttonNeg != EButton.EButton_None && Input.GetKey(jStr + (int)kp.buttonNeg)) mag -= 1.0f;
                        if (kp.buttonPos != EButton.EButton_None && Input.GetKey(jStr + (int)kp.buttonPos)) mag += 1.0f;
                        // Controller - axes
                        mag += Input.GetAxis("joystick " + m_DeviceId + " axis " + (int)kp.axis);

                        return mag;
                    }
                    return 0.0f;
                case EKeyMode.EKeyMode_DualInputDevicePrioritizeController:
                    {
                        float mag = 0.0f;
                        
                        // Controller - buttons
                        string jStr = "joystick " + m_DeviceId + " button ";
                        if (kp.buttonNeg != EButton.EButton_None && Input.GetKey(jStr + (int)kp.buttonNeg)) mag -= 1.0f;
                        if (kp.buttonPos != EButton.EButton_None && Input.GetKey(jStr + (int)kp.buttonPos)) mag += 1.0f;
                        // Controller - axes
                        mag += Input.GetAxis("joystick " + m_DeviceId + " axis " + (int)kp.axis);
                        // Keyboard
                        if (Input.GetKey(kp.keyNeg)) mag -= 1.0f;
                        if (Input.GetKey(kp.keyPos)) mag += 1.0f;

                        return mag;
                    }
                    return 0.0f;
            }
            return 0.0f;
        }
        public Vector2 GetPressAsAxis(bool left)
        {
            float hPress;
            float vPress; 
            if(left)
            {
                hPress= GetPress(EKeyPairId.EKeyPairId_HorizontalLeft);
                vPress = GetPress(EKeyPairId.EKeyPairId_VerticalLeft);
            }
            else
            {
                hPress = GetPress(EKeyPairId.EKeyPairId_HorizontalRight);
                vPress = GetPress(EKeyPairId.EKeyPairId_VerticalRight);
            }

            return new Vector2(hPress, vPress);
        }
        
        //======================
        // Bindings
        //======================
        [SerializeField] Key     m_Action1;
        [SerializeField] Key     m_Action2;

        [SerializeField] Key     m_Confirm;
        [SerializeField] Key     m_Cancel;
        [SerializeField] Key     m_Start;

        [SerializeField] KeyPair m_HorizontalLeft;
        [SerializeField] KeyPair m_VerticalLeft;

        [SerializeField] KeyPair m_HorizontalRight;
        [SerializeField] KeyPair m_VerticalRight;

        Key GetKey(EKeyId a_Id)
        {
            switch (a_Id)
            {
                case EKeyId.EKeyId_Action1:
                    return m_Action1;
                case EKeyId.EKeyId_Action2:
                    return m_Action2;

                case EKeyId.EKeyId_Confirm:
                    return m_Confirm;
                case EKeyId.EKeyId_Cancel:
                    return m_Cancel;
                case EKeyId.EKeyId_Start:
                    return m_Start;
                    
                default:
                    Debug.LogWarning("Invalid key ID passed");
                    return new Key();
            }
        }
        KeyPair GetKeyPair(EKeyPairId a_Id)
        {
            switch (a_Id)
            {
                case EKeyPairId.EKeyPairId_HorizontalLeft:
                    return m_HorizontalLeft;
                case EKeyPairId.EKeyPairId_VerticalLeft:
                    return m_VerticalLeft;
                case EKeyPairId.EKeyPairId_HorizontalRight:
                    return m_HorizontalRight;
                case EKeyPairId.EKeyPairId_VerticalRight:
                    return m_VerticalRight;

                default:
                    Debug.LogWarning("Invalid keypair ID passed");
                    return new KeyPair();
            }
        }

        public void SetKey(EKeyId a_Id, KeyCode a_Key)
        {
            Key k   = GetKey(a_Id);
            k.state = Key.EState.EState_KeyboardButton;
            k.key   = a_Key;
            
        }
        public void SetKey(EKeyId a_Id, EButton a_Button)
        {
            Key k    = GetKey(a_Id);
            k.state  = Key.EState.EState_JoystickButton;
            k.button = a_Button;
        }

        public void SetKeyPairHalf(EKeyPairId a_Id, KeyCode a_Key, EKeyPairHalf a_KeyPairHalf)
        {
            KeyPair kp = GetKeyPair(a_Id);
            kp.state  = KeyPair.EState.EState_KeyboardButtonPair;

            if (a_KeyPairHalf == EKeyPairHalf.EKeyPairHalf_Neg)
                kp.keyNeg = a_Key;
            else
                kp.keyPos = a_Key;
        }
        public void SetKeyPairHalf(EKeyPairId a_Id, EButton a_Button, EKeyPairHalf a_KeyPairHalf)
        {
            KeyPair kp = GetKeyPair(a_Id);
            kp.state = KeyPair.EState.EState_JoystickButtonPair;

            if (a_KeyPairHalf == EKeyPairHalf.EKeyPairHalf_Neg)
                kp.buttonNeg = a_Button;
            else
                kp.buttonPos = a_Button;
        }

        public void SetKeyPair(EKeyPairId a_Id, KeyCode a_KeyNeg, KeyCode a_KeyPos)
        {
            KeyPair kp = GetKeyPair(a_Id);
            kp.state  = KeyPair.EState.EState_KeyboardButtonPair;
            kp.keyNeg = a_KeyNeg;
            kp.keyPos = a_KeyPos;
        }
        public void SetKeyPair(EKeyPairId a_Id, EButton a_ButtonNeg, EButton a_ButtonPos)
        {
            KeyPair kp = GetKeyPair(a_Id);
            kp.state     = KeyPair.EState.EState_KeyboardButtonPair;
            kp.buttonNeg = a_ButtonNeg;
            kp.buttonPos = a_ButtonPos;
        }
        public void SetKeyPair(EKeyPairId a_Id, EAxis a_Axis)
        {
            KeyPair kp = GetKeyPair(a_Id);
            kp.state = KeyPair.EState.EState_JoystickAxis;
            kp.axis = a_Axis;
        }

    };
};