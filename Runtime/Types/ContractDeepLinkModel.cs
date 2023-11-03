namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        using AlephVault.Unity.DeepLinks.Types;

        /// <summary>
        ///   Model to interact with a contract.
        /// </summary>
        public abstract class ContractDeepLinkModel : DeepLinkModel
        {
            /// <summary>
            ///   The contract address.
            /// </summary>
            public readonly string ContractAddress;

            public ContractDeepLinkModel(string contractAddress)
            {
                ContractAddress = contractAddress;
            }
        }
    }
}
