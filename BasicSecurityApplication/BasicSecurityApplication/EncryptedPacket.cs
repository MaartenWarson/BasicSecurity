using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSecurityApplication
{
    //OPSLAAN VAN GEENCRYPTEERDE BOODSCHAP, SLEUTEL en IV
    class EncryptedPacket
    {
        public byte[] encryptedSessionKey;
        public byte[] encryptedData;
        public byte[] iv;
    }
}
