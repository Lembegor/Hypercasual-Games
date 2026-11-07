using System;

namespace Game.Core
{
    public class GameController
    {
        public event Action<GameState> Finished;

        public GameState State { get; private set; } = GameState.Playing;

        public void Win()
        {
            Finish(GameState.Won);
        }

        public void Lose()
        {
            Finish(GameState.Lost);
        }

        private void Finish(GameState state)
        {
            if (State != GameState.Playing)
                return;

            State = state;
            Finished?.Invoke(state);
        }
    }
}
