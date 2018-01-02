namespace Game.ActorModel.Messages
{
    public class AttackPlayerMessage
    {
        public AttackPlayerMessage(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; private set; }
    }
}
