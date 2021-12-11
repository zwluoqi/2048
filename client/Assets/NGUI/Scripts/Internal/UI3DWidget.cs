//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for all UI components that should be derived from when creating new widget types.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI 3DWidget")]
public class UI3DWidget : UIWidget
{

    public override int minWidth { get { return 1; } }

    override public int minHeight { get { return 1; } }

}
