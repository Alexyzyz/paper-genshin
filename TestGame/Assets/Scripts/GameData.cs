using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{

    public enum Element
    {
        NONE,
        PYRO,
        HYDRO,
        CRYO,
        ANEMO,
    }

    public enum Reaction
    {
        NONE,
        
        VAPORIZE,   // PYRO x HYDRO
        MELT,       // PYRO x CRYO

        FROZEN,     // HYDRO x CRYO

        SWIRL,      // PYRO, HYDRO, CRYO x ANEMO
    }

}
