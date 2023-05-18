using Sprint1.Audio;

namespace Sprint1.Commands
{
    internal class MuteCommand : ICommand
    {
        AudioManager receiver;

        public MuteCommand(AudioManager reciever)
        {
            this.receiver = reciever;
        }

        public void Execute()
        {
            receiver.Mute();
        }
    }
}

