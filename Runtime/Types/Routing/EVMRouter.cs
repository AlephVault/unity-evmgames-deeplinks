using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3;
using AlephVault.Unity.DeepLinks.Types.Routing;

namespace AlephVault.Unity.EVMGames.DeepLinks
{
    namespace Types
    {
        namespace Routing
        {
            /// <summary>
            ///   An EVM router includes some parsing / exporting
            ///   methods out of the box (e.g. addresses, amounts,
            ///   and token ids).
            /// </summary>
            public class EVMRouter : Router
            {
                /// <summary>
                ///   Tries to parse a string as a big integer.
                /// </summary>
                /// <param name="text">The text to parse</param>
                /// <param name="value">The parsed value</param>
                /// <param name="allowEthNotation">Whether to allow "XXXeth" values or not</param>
                /// <returns>Whether it could parse it or not</returns>
                protected bool IsBigInt(string text, out BigInteger value, bool allowEthNotation = true)
                {
                    value = new BigInteger(-1);
                    if (text == null) return false;
                    text = text.ToLower();

                    try
                    {
                        // The token can be a 1 to 64 hex digits string.
                        string pattern = allowEthNotation
                            ? @"^([a-fA-F0-9]{1,64}|\d+(\.\d+)?eth)$" : @"^[a-fA-F0-9]{1,64}$";
                        if (new Regex(pattern).Match(text).Success)
                        {
                            if (text.EndsWith("eth"))
                            {
                                value = Web3.Convert.ToWei(text.Substring(0, text.Length - 3));
                            }
                            else
                            {
                                value = BigInteger.Parse("0" + text, NumberStyles.HexNumber);
                            }
                            return true;
                        }

                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                /// <summary>
                ///   Exports a big integer amount to eth or hexadecimal wei.
                /// </summary>
                /// <param name="value">The value to export</param>
                /// <returns>The value to export</returns>
                protected string ExportAmount(BigInteger value)
                {
                    if (value % 100000000000000 == 0)
                    {
                        return $"{Web3.Convert.FromWei(value)}eth";
                    }
                    return value.ToHex(false).Substring(2);
                }

                /// <summary>
                ///   Tries to parse this value as a string.
                /// </summary>
                /// <param name="address">The text to parse</param>
                /// <returns>Whether it could parse it or not</returns>
                protected bool IsAddress(string address)
                {
                    if (address == null) return false;
                    
                    try
                    {
                        // The address is a 40 hex digit string with valid capitalization.
                        return new Regex(@"^[a-fA-F0-9]{40}$").Match(address).Success &&
                               new AddressUtil().IsValidEthereumAddressHexFormat("0x" + address);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                /// <summary>
                ///   Tries to parse a generic bulk of tokens, expressed
                ///   in [{token:bulk},...] syntax of at least only one
                ///   element (and, perhaps, two or more).
                /// </summary>
                /// <param name="bulk">The text to parse</param>
                /// <param name="tokens">The bulk</param>
                /// <param name="aliases">Optional aliases to allow for token ids</param>
                /// <returns>Whether it could parse it or not</returns>
                protected bool IsBulk(
                    string bulk, out Tuple<BigInteger, BigInteger>[] tokens,
                    Dictionary<string, BigInteger> aliases = null
                ) {
                    tokens = null;
                    if (bulk == null) return false;
                    string[] items = bulk.Split(",");
                    List<Tuple<BigInteger, BigInteger>> parsedItems = new List<Tuple<BigInteger, BigInteger>>();

                    string valuePattern = @"([a-fA-F0-9]{1,64}|\d+(\.\d+)?eth)";
                    string idPattern = @"[a-fA-F0-9]{1,64}";
                    if (aliases != null)
                    {
                        idPattern = $"({idPattern}|~({string.Join("|", aliases.Keys)}))";
                    }
                    
                    try
                    {
                        foreach (string item in items)
                        {
                            if (!new Regex($"^{idPattern}:{valuePattern}$").Match(item).Success)
                            {
                                return false;
                            }

                            string[] parts = item.Split(":");
                            BigInteger id;
                            if (aliases == null || !parts[0].StartsWith("~"))
                            {
                                id = BigInteger.Parse("0" + parts[0], NumberStyles.HexNumber);
                            }
                            else if (aliases.TryGetValue(parts[0].Substring(1), out id))
                            {
                                // Nothing here.
                            }

                            BigInteger amount;
                            if (parts[1].EndsWith("eth"))
                            {
                                amount = Web3.Convert.ToWei(parts[1].Substring(0, parts[1].Length - 3));
                            }
                            else
                            {
                                amount = BigInteger.Parse("0" + parts[1], NumberStyles.HexNumber);
                            }
                            
                            parsedItems.Add(new Tuple<BigInteger, BigInteger>(id, amount));
                        }

                        tokens = parsedItems.ToArray();
                        return true;
                    }
                    catch (Exception)
                    {
                        return true;
                    }
                }
            }
        }
    }
}