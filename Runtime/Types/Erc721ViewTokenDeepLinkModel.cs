using System.Numerics;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to view a token in an ERC721 contract.
        /// </summary>
        public class Erc721ViewTokenDeepLinkModel : ContractDeepLinkModel
        {
            /// <summary>
            ///   The id of the token to watch.
            /// </summary>
            public readonly BigInteger Id;
            
            public Erc721ViewTokenDeepLinkModel(string contractAddress, BigInteger id) : base(contractAddress)
            {
                Id = id;
            }
        }
    }
}
