using UnityEngine;

namespace Unmoddable;

public sealed class FocusHandler : MonoBehaviour
{
    public void OnApplicationFocus(bool focused)
    {
        if (!focused && JeffBezosController.isPlayingBeatmap && JeffBezosController.canPause)
        {
            switch (JeffBezosController.pausedState)
            {
                case PausedState.InPauseMenu:
                    JeffBezosController.pausedState = PausedState.None;
                    JeffBezosController.SetTimeScale(JeffBezosController.approachTimeScale);
                    break;
                case PausedState.InJournal:
                    JeffBezosController.pausedState = PausedState.None;
                    JeffBezosController.SetTimeScale(JeffBezosController.approachTimeScale);
                    break;
                case PausedState.None:
                    var inst = JeffBezosController.instance;
                    if (inst && inst.pauseMenu)
                        JeffBezosController.pausedState = PausedState.InPauseMenu;
                    break;
            }
        }
    }
}