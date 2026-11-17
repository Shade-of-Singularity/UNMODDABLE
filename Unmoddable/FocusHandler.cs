using Rhythm;
using UnityEngine;

namespace Unmoddable;

public sealed class FocusHandler : MonoBehaviour
{
    public bool IsFocused { get; private set; } = true;

    public void Update()
    {
        if (!IsFocused) TryPauseInArcade();
    }

    public void OnApplicationFocus(bool focused)
    {
        IsFocused = focused;
        if (!focused) TryPauseInArcade();
    }

    void TryPauseInArcade()
    {
        if (JeffBezosController.pausedState != PausedState.None && JeffBezosController.canPause) return;

        // Note: RhythmController.Instance is only set when playing any beatmap.
        // Note: _play can be replaced with GetHasStartedPlaying() method, in case they update it.
        if (JeffBezosController.isPlayingBeatmap || (RhythmController.Instance is { _play: true }))
        {
            var inst = JeffBezosController.instance;
            if (inst && inst.pauseMenu)
                JeffBezosController.pausedState = PausedState.InPauseMenu;
        }
    }
}