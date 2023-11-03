namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to view an ERC777 contract.
        /// </summary>
        public class Erc777ViewDeepLinkModel : ContractDeepLinkModel
        {
            public Erc777ViewDeepLinkModel(string contractAddress) : base(contractAddress) {}
        }
    }
}
