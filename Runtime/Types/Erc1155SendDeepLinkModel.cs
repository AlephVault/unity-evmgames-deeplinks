using System;
using System.Numerics;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to send tokens on an ERC1155 contract.
        /// </summary>
        public class Erc1155SendDeepLinkModel : ContractDeepLinkModel
        {
            /// <summary>
            ///   The address to send the tokens to.
            /// </summary>
            public readonly string TargetAddress;

            /// <summary>
            ///   The tokens to send.
            /// </summary>
            public readonly Tuple<BigInteger, BigInteger>[] Tokens;
            
            public Erc1155SendDeepLinkModel(
                string contractAddress, string targetAddress, Tuple<BigInteger, BigInteger>[] tokens
            ) : base(contractAddress)
            {
                TargetAddress = targetAddress;
                Tokens = tokens;
            }
        }
    }
}
