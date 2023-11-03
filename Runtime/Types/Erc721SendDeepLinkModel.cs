using System.Numerics;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to send a token on an ERC721 contract.
        /// </summary>
        public class Erc721SendDeepLinkModel : ContractDeepLinkModel
        {
            /// <summary>
            ///   The address to send the token to.
            /// </summary>
            public readonly string TargetAddress;

            /// <summary>
            ///   The token to send.
            /// </summary>
            public readonly BigInteger Id;
            
            public Erc721SendDeepLinkModel(
                string contractAddress, string targetAddress, BigInteger id
            ) : base(contractAddress)
            {
                TargetAddress = targetAddress;
                Id = id;
            }
        }
    }
}
