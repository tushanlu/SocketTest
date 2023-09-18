using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest
{
    [Serializable]
    public class CProject
    {
        public List<string> m_TabPageList = new List<string>();

        public List<int> m_TimeList = new List<int>();

        public List<bool> m_SocketTypeList = new List<bool>();

        public List<List<string>> m_MessageList = new List<List<string>>();
    }

}
