using System.Numerics;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to view a token on an ERC1155 contract.
        /// </summary>
        public class Erc1155ViewTokenDeepLinkModel : ContractDeepLinkModel
        {
            /// <summary>
            ///   The id of the token to watch.
            /// </summary>
            public readonly BigInteger Id;
            
            public Erc1155ViewTokenDeepLinkModel(string contractAddress, BigInteger id) : base(contractAddress)
            {
                Id = id;
            }
        }
    }
}
