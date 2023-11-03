namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to view an ERC721 contract.
        /// </summary>
        public class Erc721ViewDeepLinkModel : ContractDeepLinkModel
        {
            public Erc721ViewDeepLinkModel(string contractAddress) : base(contractAddress) {}
        }
    }
}
