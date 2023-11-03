using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Nethereum.Hex.HexConvertors.Extensions;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types
    {
        namespace Routing
        {
            /// <summary>
            ///   A convenience class to add several pre-built
            ///   deep links: ones starting with eth://, erc20://,
            ///   erc777:// and erc1155:// intended to interact
            ///   with standard token contracts. These deep links
            ///   will ONLY prepare a transaction (the user will
            ///   choose whether to fulfill it or not).
            /// </summary>
            public class DefaultEVMRouter : EVMRouter
            {
                /// <summary>
                ///   This event processes an ERC20 View deep link.
                /// </summary>
                public event Action<Erc20ViewDeepLinkModel> OnERC20View;

                /// <summary>
                ///   This event processes an ERC20 Send deep link.
                /// </summary>
                public event Action<Erc20SendDeepLinkModel> OnERC20Send;

                /// <summary>
                ///   This event processes an ERC721 View deep link.
                /// </summary>
                public event Action<Erc721ViewDeepLinkModel> OnERC721View;

                /// <summary>
                ///   This event processes an ERC721 View Token deep link.
                /// </summary>
                public event Action<Erc721ViewTokenDeepLinkModel> OnERC721ViewToken;

                /// <summary>
                ///   This event processes an ERC721 Send deep link.
                /// </summary>
                public event Action<Erc721SendDeepLinkModel> OnERC721Send;

                /// <summary>
                ///   This event processes an ERC777 View deep link.
                /// </summary>
                public event Action<Erc777ViewDeepLinkModel> OnERC777View;

                /// <summary>
                ///   This event processes an ERC777 Send deep link.
                /// </summary>
                public event Action<Erc777SendDeepLinkModel> OnERC777Send;

                /// <summary>
                ///   This event processes an ERC1155 View Token deep link.
                /// </summary>
                public event Action<Erc1155ViewTokenDeepLinkModel> OnERC1155ViewToken;

                /// <summary>
                ///   This event processes an ERC1155 Send deep link.
                /// </summary>
                public event Action<Erc1155SendDeepLinkModel> OnERC1155Send;

                /// <summary>
                ///   This event processes an Eth Send deep link.
                /// </summary>
                public event Action<EthSendDeepLinkModel> OnEthSend;

                public DefaultEVMRouter()
                {
                    // ERC-20
                    AddParsingRule("erc20")
                        .MatchingScheme("erc20")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath("/")
                        .BuildingDeepLinkModelAs(result => new Erc20ViewDeepLinkModel(result.AuthorityMatches[0][0]));
                    OnDeepLink<Erc20ViewDeepLinkModel>(deepLinkModel => OnERC20View?.Invoke(deepLinkModel));
                    AddExportRule<Erc20ViewDeepLinkModel>(deepLinkModel => $"erc20://{deepLinkModel.ContractAddress}");

                    AddParsingRule("erc20/send")
                        .MatchingScheme("erc20")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath("/send")
                        .BuildingDeepLinkModelAs(result =>
                        {
                            string to = result.GetQueryStringParameter("to");
                            string amountStr = result.GetQueryStringParameter("amount");
                            if (!IsAddress(to) || !IsBigInt(amountStr, out BigInteger amount)) return null;
                            
                            return new Erc20SendDeepLinkModel(
                                result.AuthorityMatches[0][0], to, amount
                            );
                        });
                    OnDeepLink<Erc20SendDeepLinkModel>(deepLinkModel => OnERC20Send?.Invoke(deepLinkModel));
                    AddExportRule<Erc20SendDeepLinkModel>(deepLinkModel =>
                    {
                        string amount = ExportAmount(deepLinkModel.Amount);
                        return $"erc20://{deepLinkModel.ContractAddress}/send?to={deepLinkModel.TargetAddress}?amount={amount}";
                    });
                    
                    // ERC-721
                    AddParsingRule("erc721")
                        .MatchingScheme("erc721")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath("/")
                        .BuildingDeepLinkModelAs(result => new Erc721ViewDeepLinkModel(result.AuthorityMatches[0][0]));
                    OnDeepLink<Erc721ViewDeepLinkModel>(deepLinkModel => OnERC721View?.Invoke(deepLinkModel));
                    AddExportRule<Erc721ViewDeepLinkModel>(deepLinkModel =>
                    {
                        return $"erc721://{deepLinkModel.ContractAddress}";
                    });

                    AddParsingRule("erc721/token")
                        .MatchingScheme("erc721")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath(new Regex(@"^/([a-fA-F0-9]{1,64})$"))
                        .BuildingDeepLinkModelAs(result =>
                        {
                            IsBigInt(result.PathMatches[1][0], out BigInteger tokenId);
                            return new Erc721ViewTokenDeepLinkModel(
                                result.AuthorityMatches[0][0], tokenId
                            );
                        });
                    OnDeepLink<Erc721ViewTokenDeepLinkModel>(deepLinkModel => OnERC721ViewToken?.Invoke(deepLinkModel));
                    AddExportRule<Erc721ViewTokenDeepLinkModel>(deepLinkModel =>
                    {
                        return $"erc721://{deepLinkModel.ContractAddress}/{deepLinkModel.Id}";
                    });

                    AddParsingRule("erc721/send")
                        .MatchingScheme("erc721")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath("/send")
                        .BuildingDeepLinkModelAs(result =>
                        {
                            string to = result.GetQueryStringParameter("to");
                            string idStr = result.GetQueryStringParameter("id");
                            if (!IsAddress(to) || !IsBigInt(idStr, out BigInteger id)) return null;
                            
                            return new Erc721SendDeepLinkModel(
                                result.AuthorityMatches[0][0], to, id
                            );
                        });
                    OnDeepLink<Erc721SendDeepLinkModel>(deepLinkModel => OnERC721Send?.Invoke(deepLinkModel));
                    AddExportRule<Erc721SendDeepLinkModel>(deepLinkModel =>
                    {
                        return $"erc721://{deepLinkModel.ContractAddress}/send?to=?{deepLinkModel.TargetAddress}&id={deepLinkModel.Id}";
                    });
                    
                    // ERC-777
                    AddParsingRule("erc777")
                        .MatchingScheme("erc777")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath("/")
                        .BuildingDeepLinkModelAs(result => new Erc777ViewDeepLinkModel(result.AuthorityMatches[0][0]));
                    OnDeepLink<Erc777ViewDeepLinkModel>(deepLinkModel => OnERC777View?.Invoke(deepLinkModel));
                    AddExportRule<Erc777ViewDeepLinkModel>(deepLinkModel => $"erc777://{deepLinkModel.ContractAddress}");
                    
                    AddParsingRule("erc777/send")
                        .MatchingScheme("erc777")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath("/send")
                        .BuildingDeepLinkModelAs(result =>
                        {
                            string to = result.GetQueryStringParameter("to");
                            string amountStr = result.GetQueryStringParameter("amount");
                            if (!IsAddress(to) || !IsBigInt(amountStr, out BigInteger amount)) return null;
                            
                            return new Erc777SendDeepLinkModel(
                                result.AuthorityMatches[0][0], to, amount
                            );
                        });
                    OnDeepLink<Erc777SendDeepLinkModel>(deepLinkModel => OnERC777Send?.Invoke(deepLinkModel));
                    AddExportRule<Erc777SendDeepLinkModel>(deepLinkModel =>
                    {
                        string amount = ExportAmount(deepLinkModel.Amount);
                        return $"erc777://{deepLinkModel.ContractAddress}/send?to={deepLinkModel.TargetAddress}?amount={amount}";
                    });
                    
                    // ERC-1155
                    AddParsingRule("erc1155/token")
                        .MatchingScheme("erc1155")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath(new Regex(@"^/([a-fA-F0-9]{1,64})$"))
                        .BuildingDeepLinkModelAs(result =>
                        {
                            IsBigInt(result.PathMatches[1][0], out BigInteger tokenId);
                            return new Erc1155ViewTokenDeepLinkModel(
                                result.AuthorityMatches[0][0], tokenId
                            );
                        });
                    OnDeepLink<Erc1155ViewTokenDeepLinkModel>(deepLinkModel => OnERC1155ViewToken?.Invoke(deepLinkModel));
                    AddExportRule<Erc1155ViewTokenDeepLinkModel>(deepLinkModel =>
                    {
                        return $"erc1155://{deepLinkModel.ContractAddress}/{deepLinkModel.Id}";
                    });

                    AddParsingRule("erc1155/send")
                        .MatchingScheme("erc1155")
                        .MatchingAuthority(new Regex(@"^[a-fA-F0-9]{40}$"))
                        .MatchingPath("/send")
                        .BuildingDeepLinkModelAs(result =>
                        {
                            string to = result.GetQueryStringParameter("to");
                            string bulk = result.GetQueryStringParameter("bulk");
                            if (!IsAddress(to)) return null;
                            if (!IsBulk(bulk, out Tuple<BigInteger, BigInteger>[] items)) return null;
                            
                            return new Erc1155SendDeepLinkModel(
                                result.AuthorityMatches[0][0], to, items
                            );
                        });
                    OnDeepLink<Erc1155SendDeepLinkModel>(deepLinkModel => OnERC1155Send?.Invoke(deepLinkModel));
                    AddExportRule<Erc1155SendDeepLinkModel>(deepLinkModel =>
                    {
                        string bulk = string.Join(
                            ",", deepLinkModel.Tokens.Select(pair =>
                            {
                                string id = pair.Item1.ToHex(false).Substring(2);
                                string amount = ExportAmount(pair.Item2);
                                return $"{id}:{amount}";
                            }).ToArray()
                        );
                        return $"erc1155://{deepLinkModel.ContractAddress}/send?to={deepLinkModel.TargetAddress}?bulk={bulk}";
                    });
                    
                    // ETH
                    AddParsingRule("eth/send")
                        .MatchingScheme("eth")
                        .MatchingAuthority("")
                        .MatchingPath("/send")
                        .BuildingDeepLinkModelAs(result =>
                        {
                            string to = result.GetQueryStringParameter("to");
                            string amountStr = result.GetQueryStringParameter("amount");
                            if (!IsAddress(to) || !IsBigInt(amountStr, out BigInteger amount)) return null;
                            
                            return new EthSendDeepLinkModel(to, amount);
                        });
                    OnDeepLink<EthSendDeepLinkModel>(deepLinkModel => OnEthSend?.Invoke(deepLinkModel));
                    AddExportRule<EthSendDeepLinkModel>(deepLinkModel =>
                    {
                        string amount = ExportAmount(deepLinkModel.Amount);
                        return $"eth:/send?to={deepLinkModel.TargetAddress}&amount={amount}";
                    });
                }
            }
        }
    }
}