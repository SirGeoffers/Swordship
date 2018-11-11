using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {

    public class ButtonGoto : MonoBehaviour {

        public void Goto(MenuScreen _screen) {
            MenuManager.Instance.LoadScreen(_screen);
        }

    }

}


