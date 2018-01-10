using System;
using System.Text;

namespace TVNK
{
    public static class CasparCommand
    {
        public static string Load(int channel, int layer, string clip, string parameters = null)
        {
            return BuildCommand("LOAD", channel, layer, clip, parameters);
        }

        public static string Play(int channel, int layer, string clip, string parameters = null)
        {
            return BuildCommand("PLAY",channel,layer,clip,parameters);
        }

        public static string Stop(int channel, int layer)
        {
            return BuildCommand("STOP", channel, layer, null, null);
        }

        public static string Resume(int channel, int layer)
        {
            return BuildCommand("RESUME", channel, layer, null, null);
        }

        public static string Clear(int channel, int layer)
        {
            return BuildCommand("CLEAR", channel, layer, null, null);
        }

        public static string Call(int channel, int layer, string parameters)
        {
            return BuildCommand("CALL", channel, layer, null, parameters);
        }

        public static string Invoke(int channel, int layer, string parameters)
        {
            return BuildCommand("INVOKE", channel, layer, null, parameters);
        }

        private static string BuildCommand(string command, int channel, int layer, string clip, string parameters = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(command);
            sb.Append(" ");
            sb.Append(channel);
            sb.Append("-");
            sb.Append(layer);
            if (!String.IsNullOrEmpty(clip))
            {
                sb.Append(" ");
                sb.Append(clip);
            }
            if (!String.IsNullOrEmpty(parameters))
            {
                sb.Append(" ");
                sb.Append(parameters);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
