using System.Numerics;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        using AlephVault.Unity.DeepLinks.Types;

        /// <summary>
        ///   Model to send some eth (native token) to an address.
        /// </summary>
        public class EthSendDeepLinkModel : DeepLinkModel
        {
            /// <summary>
            ///   The address to send an amount to.
            /// </summary>
            public readonly string TargetAddress;

            /// <summary>
            ///   The amount to send.
            /// </summary>
            public readonly BigInteger Amount;
            
            public EthSendDeepLinkModel(
                string targetAddress, BigInteger amount
            )
            {
                TargetAddress = targetAddress;
                Amount = amount;
            }
        }
    }
}
