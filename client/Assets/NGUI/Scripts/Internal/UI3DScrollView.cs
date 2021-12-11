//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// This script, when attached to a panel turns it into a scroll view.
/// You can then attach UIDragScrollView to colliders within to make it draggable.
/// </summary>

//add by zhouwei

[ExecuteInEditMode]
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Interaction/Scroll View 3D")]
public class UI3DScrollView : UIScrollView
{

    public override  void Press(bool pressed)
    {
        if (UICamera.currentScheme == UICamera.ControlScheme.Controller) return;

        if (smoothDragStart && pressed)
        {
            mDragStarted = false;
            mDragStartOffset = Vector2.zero;
        }

        if (enabled && NGUITools.GetActive(gameObject))
        {
            if (!pressed && mDragID == UICamera.currentTouchID) mDragID = -10;

            mCalculatedBounds = false;
            mShouldMove = shouldMove;
            if (!mShouldMove) return;
            mPressed = pressed;

            if (pressed)
            {
                // Remove all momentum on press
                mMomentum = Vector3.zero;
                mScroll = 0f;

                // Disable the spring movement
                DisableSpring();

                // Remember the hit position
                mLastPos = UICamera.lastWorldPosition;

                // Create the plane to drag along
                mPlane = new Plane(mTrans.rotation * Vector3.back, mLastPos);

                // Ensure that we're working with whole numbers, keeping everything pixel-perfect
                Vector2 co = mPanel.clipOffset;
                //co.x = Mathf.Round(co.x);
                //co.y = Mathf.Round(co.y);
                mPanel.clipOffset = co;

                Vector3 v = mTrans.localPosition;
                //v.x = Mathf.Round(v.x);
                //v.y = Mathf.Round(v.y);
                mTrans.localPosition = v;

                if (!smoothDragStart)
                {
                    mDragStarted = true;
                    mDragStartOffset = Vector2.zero;
                    if (onDragStarted != null) onDragStarted();
                }
            }
            else
            {
                if (centerOnChild != null)
                {
                    centerOnChild.Recenter();
                }
                else
                {
                    if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None)
                        RestrictWithinBounds(dragEffect == DragEffect.None, canMoveHorizontally, canMoveVertically);

                    if (mDragStarted && onDragFinished != null) onDragFinished();
                    if (!mShouldMove && onStoppedMoving != null)
                        onStoppedMoving();
                }
            }
        }
    }
}
