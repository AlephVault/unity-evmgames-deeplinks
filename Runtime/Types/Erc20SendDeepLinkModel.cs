using System.Numerics;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types 
    {
        /// <summary>
        ///   Model to send a token amount on an ERC20 contract.
        /// </summary>
        public class Erc20SendDeepLinkModel : ContractDeepLinkModel
        {
            /// <summary>
            ///   The address to send an amount to.
            /// </summary>
            public readonly string TargetAddress;

            /// <summary>
            ///   The amount to send.
            /// </summary>
            public readonly BigInteger Amount;
            
            public Erc20SendDeepLinkModel(
                string contractAddress, string targetAddress, BigInteger amount
            ) : base(contractAddress)
            {
                TargetAddress = targetAddress;
                Amount = amount;
            }
        }
    }
}
