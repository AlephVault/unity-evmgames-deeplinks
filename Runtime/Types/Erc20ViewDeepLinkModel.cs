namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to view an ERC20 contract.
        /// </summary>
        public class Erc20ViewDeepLinkModel : ContractDeepLinkModel
        {
            public Erc20ViewDeepLinkModel(string contractAddress) : base(contractAddress) {}
        }
    }
}
