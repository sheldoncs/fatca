using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace fatca
{
    public static class FatcaLibrary
    {
        private static XmlNode readInTemplate(string path, Variables.FatcaData fatca)
        {
            
           
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(path);

           
            XmlNode rootNode = doc.SelectSingleNode("ftc:FATCA_OECD");
            XmlNode AccountReportNode = rootNode.FirstChild;

                XmlNode DocRefIdNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:DocSpec/ftc:DocRefId");
                DocRefIdNode.AppendChild(doc.CreateTextNode(fatca.docrefid));

                XmlNode AccountNumberNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountNumber");
                AccountNumberNode.AppendChild(doc.CreateTextNode(fatca.accountno));
                
                XmlNode ResCountryNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:ResCountryCode");
                ResCountryNode.AppendChild(doc.CreateTextNode(fatca.resCountryCode));

                XmlNode TINNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:TIN");
                TINNode.AppendChild(doc.CreateTextNode(fatca.tin));
                XmlAttribute issuedByAttr = doc.CreateAttribute("issuedBy");
                issuedByAttr.Value = fatca.tinIssuedBy;
                TINNode.Attributes.Append(issuedByAttr);

                XmlNode FirstNameNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:Name/sfa:FirstName");
                FirstNameNode.AppendChild(doc.CreateTextNode(fatca.firstname));

                XmlNode LastNameNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:Name/sfa:LastName");
                LastNameNode.AppendChild(doc.CreateTextNode(fatca.lastname));

                XmlNode CountryNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:Address/sfa:CountryCode");
                CountryNode.AppendChild(doc.CreateTextNode(fatca.countrycode));

                XmlNode StreetNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:Address/sfa:AddressFix/sfa:Street");
                StreetNode.AppendChild(doc.CreateTextNode(fatca.street));

                XmlNode PostCodeNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:Address/sfa:AddressFix/sfa:PostCode");
                PostCodeNode.AppendChild(doc.CreateTextNode(fatca.zip));

                XmlNode CityNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:Address/sfa:AddressFix/sfa:City");
                CityNode.AppendChild(doc.CreateTextNode(fatca.city));

                XmlNode BirthInfo = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountHolder/ftc:Individual/sfa:BirthInfo");
                BirthInfo.AppendChild(doc.CreateTextNode(fatca.dob));

                XmlNode AccountBalanceNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:AccountBalance");
                AccountBalanceNode.AppendChild(doc.CreateTextNode(fatca.amount));
                XmlAttribute AccountBalanceAttr = doc.CreateAttribute("currCode");
                AccountBalanceAttr.Value = "BBD";
                AccountBalanceNode.Attributes.Append(AccountBalanceAttr);

                XmlNode TypeNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:Payment/ftc:Type");
                TypeNode.AppendChild(doc.CreateTextNode("FATCA502"));

                XmlNode PaymentAmntNode = doc.SelectSingleNode("/ftc:AccountReport/ftc:Payment/ftc:PaymentAmnt");
                PaymentAmntNode.AppendChild(doc.CreateTextNode(fatca.interest));
                XmlAttribute PaymentAmntAttr = doc.CreateAttribute("currCode");
                PaymentAmntAttr.Value = "BBD";
                PaymentAmntNode.Attributes.Append(PaymentAmntAttr);

                

                return AccountReportNode;
            
        }

        private static XmlNode createCRSTemplate(XmlNode crs_ReportingGroup, XmlDocument doc, List<Variables.FatcaData> list)
        {


            /*Array of AccountReport*/
            List<Variables.FatcaData>.Enumerator e = list.GetEnumerator();

            while (e.MoveNext())
            {
                Variables.FatcaData fatca = e.Current;
                XmlNode AccountReportNode = doc.CreateElement("crs", "AccountReport", "urn:oecd:ties:crs:v1");
                crs_ReportingGroup.AppendChild(AccountReportNode);

                /*ftc:DocSpec parent ftc:AccountReport*/
                XmlNode DocSpecNode = doc.CreateElement("crs", "DocSpec", "urn:oecd:ties:crs:v1");
                AccountReportNode.AppendChild(DocSpecNode);

                XmlNode DocTypeIndic = doc.CreateElement("stf", "DocTypeIndic", "urn:oecd:ties:stf:v4");
                DocTypeIndic.AppendChild(doc.CreateTextNode("OECD1"));
                DocSpecNode.AppendChild(DocTypeIndic);


                XmlNode DocRefIdNode = doc.CreateElement("stf", "DocRefId", "urn:oecd:ties:stf:v4");
                DocRefIdNode.AppendChild(doc.CreateTextNode(fatca.docrefid));
                DocSpecNode.AppendChild(DocRefIdNode);
                /*Only Use for Correction*/
                /*
                XmlNode CorrMessageRefIdNode = doc.CreateElement("ftc", "CorrMessageRefId", "urn:oecd:ties:fatca:v2");
                DocSpecNode.AppendChild(CorrMessageRefIdNode);

                XmlNode CorrDocRefIdNode = doc.CreateElement("ftc", "CorrDocRefId", "urn:oecd:ties:fatca:v2");
                DocSpecNode.AppendChild(CorrDocRefIdNode);
                */
                /*ftc:DocSpec parent ftc:AccountReport*/

                /*ftc:AccountNumber parent ftc:AccountReport*/
                XmlNode AccountNumberNode = doc.CreateElement("crs", "AccountNumber", "urn:oecd:ties:crs:v1");
                //AcctNumberType="OECD601"
                XmlAttribute AcctNumberAttr = doc.CreateAttribute("AcctNumberType");
                AcctNumberAttr.Value = "OECD601";
                AccountNumberNode.Attributes.Append(AcctNumberAttr);
                AccountNumberNode.AppendChild(doc.CreateTextNode(fatca.accountno));
                AccountReportNode.AppendChild(AccountNumberNode);
                /*ftc:AccountNumber parent ftc:AccountReport*/

                /*ftc:AccountHolder parent ftc:AccountReport*/
                XmlNode AccountHolderNode = doc.CreateElement("crs", "AccountHolder", "urn:oecd:ties:crs:v1");
                AccountReportNode.AppendChild(AccountHolderNode);
                /*ftc:AccountHolder parent ftc:AccountReport*/

                /*ftc:Individual parent ftc:AccountHolder*/
                XmlNode IndividualNode = doc.CreateElement("crs", "Individual", "urn:oecd:ties:crs:v1");
                AccountHolderNode.AppendChild(IndividualNode);
                /*ftc:Individual parent ftc:AccountHolder*/

                /*sfa:ResCountryCode parent ftc:Individual*/
                XmlNode ResCountryCodeNode = doc.CreateElement("crs", "ResCountryCode", "urn:oecd:ties:crs:v1");
                ResCountryCodeNode.AppendChild(doc.CreateTextNode(fatca.countrycode));
                IndividualNode.AppendChild(ResCountryCodeNode);
                /*sfa:ResCountryCode parent ftc:Individual*/

                /*sfa:TIN parent ftc:Individual*/
                XmlNode TINNode = doc.CreateElement("crs", "TIN", "urn:oecd:ties:crs:v1");
                if (fatca.tin.Equals(""))
                {
                    XmlAttribute tinAttr = doc.CreateAttribute("issuedBy");
                    tinAttr.Value = fatca.countrycode;
                    TINNode.Attributes.Append(tinAttr);
                }
                else
                {
                    TINNode.AppendChild(doc.CreateTextNode(fatca.tin));
                    XmlAttribute tinAttr = doc.CreateAttribute("issuedBy");
                    tinAttr.Value = fatca.countrycode;
                    TINNode.Attributes.Append(tinAttr);
                }
                IndividualNode.AppendChild(TINNode);
                /*sfa:TIN parent ftc:Individual*/

                /*sfa:Name parent ftc:Individual*/
                XmlNode NameNode = doc.CreateElement("crs", "Name", "urn:oecd:ties:crs:v1");
                IndividualNode.AppendChild(NameNode);
                /*sfa:Name parent ftc:Individual*/

                /*sfa:FirstName parent sfa:Name*/
                XmlNode FirstNameNode = doc.CreateElement("crs", "FirstName", "urn:oecd:ties:crs:v1");
                FirstNameNode.AppendChild(doc.CreateTextNode(fatca.firstname));
                NameNode.AppendChild(FirstNameNode);
                /*sfa:FirstName parent sfa:Name*/

                /*sfa:LastName parent sfa:Name*/
                XmlNode LastNameNode = doc.CreateElement("crs", "LastName", "urn:oecd:ties:crs:v1");
                LastNameNode.AppendChild(doc.CreateTextNode(fatca.lastname));
                NameNode.AppendChild(LastNameNode);
                /*sfa:LastName parent sfa:Name*/

                /*sfa:Address parent ftc:Individual*/
                XmlNode AddressNode = doc.CreateElement("crs", "Address", "urn:oecd:ties:crs:v1");
                IndividualNode.AppendChild(AddressNode);
                /*sfa:Address parent ftc:Individual*/

                /*sfa:CountryCode parent sfa:Address*/
                XmlNode CountryCodeNode = doc.CreateElement("cfc", "CountryCode", "urn:oecd:ties:commontypesfatcacrs:v1");
                CountryCodeNode.AppendChild(doc.CreateTextNode(fatca.countrycode));
                AddressNode.AppendChild(CountryCodeNode);
                /*sfa:CountryCode parent sfa:Address*/

                
                /*sfa:AddressFix parent sfa:Address*/
                XmlNode AddressFixNode = doc.CreateElement("cfc", "AddressFix", "urn:oecd:ties:commontypesfatcacrs:v1");
                AddressNode.AppendChild(AddressFixNode);
                /*sfa:CountryCode parent sfa:Address*/

                /*sfa:Street parent sfa:AddressFix*/
                XmlNode StreetNode = doc.CreateElement("cfc", "Street", "urn:oecd:ties:commontypesfatcacrs:v1");
                StreetNode.AppendChild(doc.CreateTextNode(fatca.street));
                AddressFixNode.AppendChild(StreetNode);
                /*sfa:Street parent sfa:AddressFix*/

                /*sfa:PostCode parent sfa:AddressFix*/
                XmlNode PostCodeNode = doc.CreateElement("cfc", "PostCode", "urn:oecd:ties:commontypesfatcacrs:v1");
                PostCodeNode.AppendChild(doc.CreateTextNode(fatca.zip));
                AddressFixNode.AppendChild(PostCodeNode);
                /*sfa:PostCode parent sfa:AddressFix*/

                /*cfc:City parent sfa:AddressFix*/
                XmlNode CityNode = doc.CreateElement("cfc", "City", "urn:oecd:ties:commontypesfatcacrs:v1");
                CityNode.AppendChild(doc.CreateTextNode(fatca.city));
                AddressFixNode.AppendChild(CityNode);
                /*cfc:City parent sfa:AddressFix*/
               
                /*sfa:BirthDate parent sfa:BirthInfo*/
                XmlNode BirthInfoNode = doc.CreateElement("crs", "BirthInfo", "urn:oecd:ties:crs:v1");
                IndividualNode.AppendChild(BirthInfoNode);

                XmlNode BirthDateNode = doc.CreateElement("crs", "BirthDate", "urn:oecd:ties:crs:v1");
                BirthDateNode.AppendChild(doc.CreateTextNode(fatca.dob));
                BirthInfoNode.AppendChild(BirthDateNode);
                /*sfa:BirthDate parent sfa:BirthInfo*/

                /*sfa:City parent sfa:BirthInfo*/
                XmlNode BirthCityNode = doc.CreateElement("crs", "City", "urn:oecd:ties:crs:v1");
                BirthCityNode.AppendChild(doc.CreateTextNode(fatca.city));
                BirthInfoNode.AppendChild(BirthCityNode);
                /*sfa:City parent sfa:BirthInfo*/

                
                /*ftc:AccountBalance parent ftc:AccountReport*/
                XmlNode AccountBalanceNode = doc.CreateElement("crs", "AccountBalance", "urn:oecd:ties:crs:v1");
                AccountBalanceNode.AppendChild(doc.CreateTextNode(fatca.amount));
                XmlAttribute AccountBalanceAttr = doc.CreateAttribute("currCode");
                AccountBalanceAttr.Value = fatca.currencyCode;
                AccountBalanceNode.Attributes.Append(AccountBalanceAttr);

                AccountReportNode.AppendChild(AccountBalanceNode);
                /*ftc:AccountBalance parent ftc:AccountReport*/

                /*ftc:Payment parent ftc:AccountReport*/
                XmlNode PaymentNode = doc.CreateElement("crs", "Payment", "urn:oecd:ties:crs:v1");
                AccountReportNode.AppendChild(PaymentNode);
                /*ftc:Payment parent ftc:AccountReport*/

                /*ftc:Type parent ftc:Payment*/
                XmlNode TypeNode = doc.CreateElement("crs", "Type", "urn:oecd:ties:crs:v1");
                TypeNode.AppendChild(doc.CreateTextNode("CRS502"));
                PaymentNode.AppendChild(TypeNode);
                /*ftc:Type parent ftc:Payment*/

                /*ftc:PaymentAmnt parent ftc:Payment*/
                XmlNode PaymentAmntNode = doc.CreateElement("crs", "PaymentAmnt", "urn:oecd:ties:crs:v1");
                PaymentAmntNode.AppendChild(doc.CreateTextNode(fatca.interest));
                XmlAttribute PaymentAmntAttr = doc.CreateAttribute("currCode");
                PaymentAmntAttr.Value = fatca.currencyCode;
                PaymentAmntNode.Attributes.Append(PaymentAmntAttr);

                PaymentNode.AppendChild(PaymentAmntNode);

            }
            return crs_ReportingGroup;
        }
        private static XmlNode createTemplate(XmlNode ftc_ReportingGroup, XmlDocument doc, List<Variables.FatcaData> list)
        {
           

            /*Array of AccountReport*/
            List<Variables.FatcaData>.Enumerator e = list.GetEnumerator();
            
            while (e.MoveNext())
            {
                Variables.FatcaData fatca = e.Current;
                XmlNode AccountReportNode = doc.CreateElement("ftc", "AccountReport", "urn:oecd:ties:fatca:v2");
                ftc_ReportingGroup.AppendChild(AccountReportNode);
                
                /*ftc:DocSpec parent ftc:AccountReport*/
                XmlNode DocSpecNode = doc.CreateElement("ftc", "DocSpec", "urn:oecd:ties:fatca:v2");
                AccountReportNode.AppendChild(DocSpecNode);

                XmlNode DocTypeIndic = doc.CreateElement("ftc", "DocTypeIndic", "urn:oecd:ties:fatca:v2");
                DocTypeIndic.AppendChild(doc.CreateTextNode("FATCA1"));
                DocSpecNode.AppendChild(DocTypeIndic);


                XmlNode DocRefIdNode = doc.CreateElement("ftc", "DocRefId", "urn:oecd:ties:fatca:v2");
                DocRefIdNode.AppendChild(doc.CreateTextNode(fatca.docrefid));
                DocSpecNode.AppendChild(DocRefIdNode);

                XmlNode CorrMessageRefIdNode = doc.CreateElement("ftc", "CorrMessageRefId", "urn:oecd:ties:fatca:v2");
                //CorrMessageRefIdNode.AppendChild(doc.CreateTextNode(fatca.docrefid));
                DocSpecNode.AppendChild(CorrMessageRefIdNode);

                XmlNode CorrDocRefIdNode = doc.CreateElement("ftc", "CorrDocRefId", "urn:oecd:ties:fatca:v2");
                //CorrDocRefIdNode.AppendChild(doc.CreateTextNode(fatca.docrefid));
                DocSpecNode.AppendChild(CorrDocRefIdNode);
                /*ftc:DocSpec parent ftc:AccountReport*/

                /*ftc:AccountNumber parent ftc:AccountReport*/
                XmlNode AccountNumberNode = doc.CreateElement("ftc", "AccountNumber", "urn:oecd:ties:fatca:v2");
                AccountNumberNode.AppendChild(doc.CreateTextNode(fatca.accountno));
                AccountReportNode.AppendChild(AccountNumberNode);
                /*ftc:AccountNumber parent ftc:AccountReport*/

                /*ftc:AccountHolder parent ftc:AccountReport*/
                XmlNode AccountHolderNode = doc.CreateElement("ftc", "AccountHolder", "urn:oecd:ties:fatca:v2");
                AccountReportNode.AppendChild(AccountHolderNode);
                /*ftc:AccountHolder parent ftc:AccountReport*/

                /*ftc:Individual parent ftc:AccountHolder*/
                XmlNode IndividualNode = doc.CreateElement("ftc", "Individual", "urn:oecd:ties:fatca:v2");
                AccountHolderNode.AppendChild(IndividualNode);
                /*ftc:Individual parent ftc:AccountHolder*/

                /*sfa:ResCountryCode parent ftc:Individual*/
                XmlNode ResCountryCodeNode = doc.CreateElement("sfa", "ResCountryCode", "urn:oecd:ties:stffatcatypes:v2");
                ResCountryCodeNode.AppendChild(doc.CreateTextNode(fatca.countrycode));
                IndividualNode.AppendChild(ResCountryCodeNode);
                /*sfa:ResCountryCode parent ftc:Individual*/

                /*sfa:TIN parent ftc:Individual*/
                XmlNode TINNode = doc.CreateElement("sfa", "TIN", "urn:oecd:ties:stffatcatypes:v2");
                TINNode.AppendChild(doc.CreateTextNode(fatca.tin));
                XmlAttribute tinAttr = doc.CreateAttribute("issuedBy");
                tinAttr.Value = "US";
                TINNode.Attributes.Append(tinAttr);

                IndividualNode.AppendChild(TINNode);
                /*sfa:TIN parent ftc:Individual*/

                /*sfa:Name parent ftc:Individual*/
                XmlNode NameNode = doc.CreateElement("sfa", "Name", "urn:oecd:ties:stffatcatypes:v2");
                IndividualNode.AppendChild(NameNode);
                /*sfa:Name parent ftc:Individual*/

                /*sfa:FirstName parent sfa:Name*/
                XmlNode FirstNameNode = doc.CreateElement("sfa", "FirstName", "urn:oecd:ties:stffatcatypes:v2");
                FirstNameNode.AppendChild(doc.CreateTextNode(fatca.firstname));
                NameNode.AppendChild(FirstNameNode);
                /*sfa:FirstName parent sfa:Name*/

                /*sfa:LastName parent sfa:Name*/
                XmlNode LastNameNode = doc.CreateElement("sfa", "LastName", "urn:oecd:ties:stffatcatypes:v2");
                LastNameNode.AppendChild(doc.CreateTextNode(fatca.lastname));
                NameNode.AppendChild(LastNameNode);
                /*sfa:LastName parent sfa:Name*/

                /*sfa:GenerationIdentifier parent sfa:Name*/
                XmlNode GenerationIdentifierNode = doc.CreateElement("sfa", "GenerationIdentifier", "urn:oecd:ties:stffatcatypes:v2");
                NameNode.AppendChild(GenerationIdentifierNode);
                /*sfa:GenerationIdentifier parent sfa:Name*/

                /*sfa:Suffix parent sfa:Name*/
                XmlNode SuffixNode = doc.CreateElement("sfa", "Suffix", "urn:oecd:ties:stffatcatypes:v2");
                NameNode.AppendChild(SuffixNode);
                /*sfa:Suffix parent sfa:Name*/

                /*sfa:GeneralSuffix parent sfa:Name*/
                XmlNode GeneralSuffixNode = doc.CreateElement("sfa", "GeneralSuffix", "urn:oecd:ties:stffatcatypes:v2");
                NameNode.AppendChild(GeneralSuffixNode);
                /*sfa:GeneralSuffix parent sfa:Name*/

                /*sfa:Address parent ftc:Individual*/
                XmlNode AddressNode = doc.CreateElement("sfa", "Address", "urn:oecd:ties:stffatcatypes:v2");
                IndividualNode.AppendChild(AddressNode);
                /*sfa:Address parent ftc:Individual*/

                /*sfa:CountryCode parent sfa:Address*/
                XmlNode CountryCodeNode = doc.CreateElement("sfa", "CountryCode", "urn:oecd:ties:stffatcatypes:v2");
                CountryCodeNode.AppendChild(doc.CreateTextNode(fatca.countrycode));
                AddressNode.AppendChild(CountryCodeNode);
                /*sfa:CountryCode parent sfa:Address*/

                /*sfa:AddressFix parent sfa:Address*/
                XmlNode AddressFixNode = doc.CreateElement("sfa", "AddressFix", "urn:oecd:ties:stffatcatypes:v2");
                AddressNode.AppendChild(AddressFixNode);
                /*sfa:CountryCode parent sfa:Address*/

                /*sfa:Street parent sfa:AddressFix*/
                XmlNode StreetNode = doc.CreateElement("sfa", "Street", "urn:oecd:ties:stffatcatypes:v2");
                StreetNode.AppendChild(doc.CreateTextNode(fatca.street));
                AddressFixNode.AppendChild(StreetNode);
                /*sfa:Street parent sfa:AddressFix*/

                /*sfa:BuildingIdentifier parent sfa:AddressFix*/
                XmlNode BuildingIdentifierNode = doc.CreateElement("sfa", "BuildingIdentifier", "urn:oecd:ties:stffatcatypes:v2");
                AddressFixNode.AppendChild(BuildingIdentifierNode);
                /*sfa:BuildingIdentifier parent sfa:AddressFix*/

                /*sfa:SuiteIdentifier parent sfa:AddressFix*/
                XmlNode SuiteIdentifierNode = doc.CreateElement("sfa", "SuiteIdentifier", "urn:oecd:ties:stffatcatypes:v2");
                AddressFixNode.AppendChild(SuiteIdentifierNode);
                /*sfa:SuiteIdentifier parent sfa:AddressFix*/

                /*sfa:SuiteIdentifier parent sfa:AddressFix*/
                XmlNode FloorIdentifierNode = doc.CreateElement("sfa", "FloorIdentifier", "urn:oecd:ties:stffatcatypes:v2");
                AddressFixNode.AppendChild(FloorIdentifierNode);
                /*sfa:SuiteIdentifier parent sfa:AddressFix*/

                /*sfa:DistrictName parent sfa:AddressFix*/
                XmlNode DistrictNameNode = doc.CreateElement("sfa", "DistrictName", "urn:oecd:ties:stffatcatypes:v2");
                AddressFixNode.AppendChild(DistrictNameNode);
                /*sfa:DistrictName parent sfa:AddressFix*/

                /*sfa:POB parent sfa:AddressFix*/
                XmlNode POBNode = doc.CreateElement("sfa", "POB", "urn:oecd:ties:stffatcatypes:v2");
                AddressFixNode.AppendChild(POBNode);
                /*sfa:POB parent sfa:AddressFix*/

                /*sfa:PostCode parent sfa:AddressFix*/
                XmlNode PostCodeNode = doc.CreateElement("sfa", "PostCode", "urn:oecd:ties:stffatcatypes:v2");
                PostCodeNode.AppendChild(doc.CreateTextNode(fatca.zip));
                AddressFixNode.AppendChild(PostCodeNode);
                /*sfa:PostCode parent sfa:AddressFix*/

                /*sfa:PostCode parent sfa:AddressFix*/
                XmlNode CityNode = doc.CreateElement("sfa", "City", "urn:oecd:ties:stffatcatypes:v2");
                CityNode.AppendChild(doc.CreateTextNode(fatca.city));
                AddressFixNode.AppendChild(CityNode);
                /*sfa:PostCode parent sfa:AddressFix*/
                
                /*sfa:POB parent sfa:AddressFix*/
                XmlNode CountrySubentityNode = doc.CreateElement("sfa", "CountrySubentity", "urn:oecd:ties:stffatcatypes:v2");
                AddressFixNode.AppendChild(CountrySubentityNode);
                /*sfa:POB parent sfa:AddressFix*/

                /*sfa:AddressFree parent sfa:Address*/
                //XmlNode AddressFreeNode = doc.CreateElement("sfa", "AddressFree", "urn:oecd:ties:stffatcatypes:v2");
                //AddressNode.AppendChild(AddressFreeNode);
                /*sfa:AddressFree parent sfa:Address*/

                /*sfa:BirthInfo parent ftc:Individual*/
                XmlNode BirthInfoNode = doc.CreateElement("sfa", "BirthInfo", "urn:oecd:ties:stffatcatypes:v2");
                IndividualNode.AppendChild(BirthInfoNode);
                /*sfa:BirthInfo parent ftc:Individual*/

                /*sfa:BirthDate parent sfa:BirthInfo*/
                XmlNode BirthDateNode = doc.CreateElement("sfa", "BirthDate", "urn:oecd:ties:stffatcatypes:v2");
                BirthDateNode.AppendChild(doc.CreateTextNode(fatca.dob));
                BirthInfoNode.AppendChild(BirthDateNode);
                /*sfa:BirthDate parent sfa:BirthInfo*/

                /*sfa:City parent sfa:BirthInfo*/
                XmlNode BirthCityNode = doc.CreateElement("sfa", "City", "urn:oecd:ties:stffatcatypes:v2");
                BirthInfoNode.AppendChild(BirthCityNode);
                /*sfa:City parent sfa:BirthInfo*/

                /*sfa:CitySubentity parent sfa:BirthInfo*/
                XmlNode CitySubentityNode = doc.CreateElement("sfa", "CitySubentity", "urn:oecd:ties:stffatcatypes:v2");
                BirthInfoNode.AppendChild(CitySubentityNode);
                /*sfa:CitySubentity parent sfa:BirthInfo*/

                /*ftc:AccountBalance parent ftc:AccountReport*/
                XmlNode AccountBalanceNode = doc.CreateElement("ftc", "AccountBalance", "urn:oecd:ties:fatca:v2");
                AccountBalanceNode.AppendChild(doc.CreateTextNode(fatca.amount));
                XmlAttribute AccountBalanceAttr = doc.CreateAttribute("currCode");
                AccountBalanceAttr.Value = fatca.currencyCode;
                AccountBalanceNode.Attributes.Append(AccountBalanceAttr);

                AccountReportNode.AppendChild(AccountBalanceNode);
                /*ftc:AccountBalance parent ftc:AccountReport*/

                /*ftc:Payment parent ftc:AccountReport*/
                XmlNode PaymentNode = doc.CreateElement("ftc", "Payment", "urn:oecd:ties:fatca:v2");
                AccountReportNode.AppendChild(PaymentNode);
                /*ftc:Payment parent ftc:AccountReport*/

                /*ftc:Type parent ftc:Payment*/
                XmlNode TypeNode = doc.CreateElement("ftc", "Type", "urn:oecd:ties:fatca:v2");
                TypeNode.AppendChild(doc.CreateTextNode("FATCA502"));
                PaymentNode.AppendChild(TypeNode);
                /*ftc:Type parent ftc:Payment*/

                /*ftc:PaymentTypeDesc parent ftc:Payment*/
                XmlNode PaymentTypeDescNode = doc.CreateElement("ftc", "PaymentTypeDesc", "urn:oecd:ties:fatca:v2");
                PaymentNode.AppendChild(PaymentTypeDescNode);
                /*ftc:PaymentTypeDesc parent ftc:Payment*/

                /*ftc:PaymentAmnt parent ftc:Payment*/
                XmlNode PaymentAmntNode = doc.CreateElement("ftc", "PaymentAmnt", "urn:oecd:ties:fatca:v2");
                PaymentAmntNode.AppendChild(doc.CreateTextNode(fatca.interest));
                XmlAttribute PaymentAmntAttr = doc.CreateAttribute("currCode");
                PaymentAmntAttr.Value = fatca.currencyCode;
                PaymentAmntNode.Attributes.Append(PaymentAmntAttr);

                PaymentNode.AppendChild(PaymentAmntNode);
                
            }
            return ftc_ReportingGroup;
        }

        public static String createCRSXML(string path, List<Variables.FatcaData> list, string absolutPath, string fatca_xls, string SendingCompanyIN, string docrefid,
            string companyName, string periodEnding, string MessageRefId, string shortaddress,
            string xmlfilename)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode CRS_OECD = doc.CreateElement("crs", "CRS_OECD", "urn:oecd:ties:crs:v1");



            /*Attributes Of CRS_OECD*/
            XmlAttribute version = doc.CreateAttribute("version");
            version.Value = "1.0";
            CRS_OECD.Attributes.Append(version);

            XmlAttribute xmlns_xsi = doc.CreateAttribute("xmlns:xsi");
            xmlns_xsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
            CRS_OECD.Attributes.Append(xmlns_xsi);

            XmlAttribute xmlns_cfc = doc.CreateAttribute("xmlns:cfc");
            xmlns_cfc.Value = "urn:oecd:ties:commontypesfatcacrs:v1";
            CRS_OECD.Attributes.Append(xmlns_cfc);

            XmlAttribute xmlns_crs = doc.CreateAttribute("xmlns:crs");
            xmlns_crs.Value = "urn:oecd:ties:crs:v1";
            CRS_OECD.Attributes.Append(xmlns_crs);

            XmlAttribute xmlns_ftc = doc.CreateAttribute("xmlns:ftc");
            xmlns_ftc.Value = "urn:oecd:ties:fatca:v1";
            CRS_OECD.Attributes.Append(xmlns_ftc);
            
            XmlAttribute xmlns_stf = doc.CreateAttribute("xmlns:stf");
            xmlns_stf.Value = "urn:oecd:ties:stf:v4";
            CRS_OECD.Attributes.Append(xmlns_stf);

            XmlAttribute xsi_schemaLocation = doc.CreateAttribute("xsi","schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            
            xsi_schemaLocation.Value = "urn:oecd:ties:crs:v1 CrsXML_v1.0.xsd";
            CRS_OECD.Attributes.Append(xsi_schemaLocation);
            /*Attributes Of CRS_OECD*/

            /*
            XmlNode script = doc.CreateElement("script");
            XmlAttribute id = doc.CreateAttribute("id");
            id.Value = "CRS_90.00000.LE.052";
            script.Attributes.Append(id);
            CRS_OECD.AppendChild(script);
            */
            /*Children of crs_MessageSpec*/
            XmlNode crs_MessageSpec = doc.CreateElement("crs", "MessageSpec", "urn:oecd:ties:crs:v1");
            XmlNode crs_SendingCompanyIN = doc.CreateElement("crs", "SendingCompanyIN", "urn:oecd:ties:crs:v1");
            crs_SendingCompanyIN.AppendChild(doc.CreateTextNode(SendingCompanyIN));
            crs_MessageSpec.AppendChild(crs_SendingCompanyIN);
            

            XmlNode crs_TransmittingCountry = doc.CreateElement("crs", "TransmittingCountry", "urn:oecd:ties:crs:v1");
            crs_TransmittingCountry.AppendChild(doc.CreateTextNode("BB"));
            crs_MessageSpec.AppendChild(crs_TransmittingCountry);

            XmlNode crs_ReceivingCountry = doc.CreateElement("crs", "ReceivingCountry", "urn:oecd:ties:crs:v1");
            crs_ReceivingCountry.AppendChild(doc.CreateTextNode("US"));
            crs_MessageSpec.AppendChild(crs_ReceivingCountry);

            XmlNode crs_MessageType = doc.CreateElement("crs", "MessageType", "urn:oecd:ties:crs:v1");
            crs_MessageType.AppendChild(doc.CreateTextNode("CRS"));
            crs_MessageSpec.AppendChild(crs_MessageType);

            XmlNode crs_Warning = doc.CreateElement("crs", "Warning", "urn:oecd:ties:crs:v1");
            crs_MessageSpec.AppendChild(crs_Warning);

            XmlNode crs_MessageRefId = doc.CreateElement("crs", "MessageRefId", "urn:oecd:ties:crs:v1");
            crs_MessageRefId.AppendChild(doc.CreateTextNode(MessageRefId));
            crs_MessageSpec.AppendChild(crs_MessageRefId);

            XmlNode crs_MessageTypeIndic = doc.CreateElement("crs", "MessageTypeIndic", "urn:oecd:ties:crs:v1");
            crs_MessageTypeIndic.AppendChild(doc.CreateTextNode("CRS701"));
            crs_MessageSpec.AppendChild(crs_MessageTypeIndic);
            
            XmlNode crs_ReportingPeriod = doc.CreateElement("crs", "ReportingPeriod", "urn:oecd:ties:crs:v1");
            crs_ReportingPeriod.AppendChild(doc.CreateTextNode(periodEnding));
            crs_MessageSpec.AppendChild(crs_ReportingPeriod);

            XmlNode crs_Timestamp = doc.CreateElement("crs", "Timestamp", "urn:oecd:ties:crs:v1");
            crs_Timestamp.AppendChild(doc.CreateTextNode(periodEnding+"T15:36:00"));
            crs_MessageSpec.AppendChild(crs_Timestamp);


            CRS_OECD.AppendChild(crs_MessageSpec);
            /*Children of crs_MessageSpec*/

            /*Children of crs_CrsBody*/
            XmlNode crs_CrsBody = doc.CreateElement("crs", "CrsBody", "urn:oecd:ties:crs:v1");

            /*Children of crs_ReportingFI*/
            XmlNode crs_ReportingFI = doc.CreateElement("crs", "ReportingFI", "urn:oecd:ties:crs:v1");

            XmlNode crs_ResCountryCode = doc.CreateElement("crs", "ResCountryCode", "urn:oecd:ties:crs:v1");
            crs_ResCountryCode.AppendChild(doc.CreateTextNode("BB"));
            crs_ReportingFI.AppendChild(crs_ResCountryCode);

            XmlNode crs_IN = doc.CreateElement("crs", "IN", "urn:oecd:ties:crs:v1");
            crs_IN.AppendChild(doc.CreateTextNode(SendingCompanyIN));
            XmlAttribute crsInAttrIssuedBy = doc.CreateAttribute("issuedBy");
            crsInAttrIssuedBy.Value = "BB";
            XmlAttribute crsInTypeAttr = doc.CreateAttribute("INType");
            crsInTypeAttr.Value = "TIN";
            crs_IN.Attributes.Append(crsInAttrIssuedBy);
            crs_IN.Attributes.Append(crsInTypeAttr);
            crs_ReportingFI.AppendChild(crs_IN);

            XmlNode crs_Name = doc.CreateElement("crs", "Name", "urn:oecd:ties:crs:v1");
            crs_Name.AppendChild(doc.CreateTextNode(companyName));
            crs_ReportingFI.AppendChild(crs_Name);

            XmlNode crs_Address = doc.CreateElement("crs", "Address", "urn:oecd:ties:crs:v1");

            XmlNode cfc_CountryCode = doc.CreateElement("cfc", "CountryCode", "urn:oecd:ties:commontypesfatcacrs:v1");
            cfc_CountryCode.AppendChild(doc.CreateTextNode("BB"));
            crs_Address.AppendChild(cfc_CountryCode);

            XmlNode cfc_AddressFree = doc.CreateElement("cfc", "AddressFree", "urn:oecd:ties:commontypesfatcacrs:v1");
            cfc_AddressFree.AppendChild(doc.CreateTextNode("Keith Bourne Complex Belmont Road"));
            crs_Address.AppendChild(cfc_AddressFree);
            crs_ReportingFI.AppendChild(crs_Address);

            XmlNode crs_DocSpec = doc.CreateElement("crs", "DocSpec", "urn:oecd:ties:crs:v1");
            XmlNode stf_DocTypeIndic = doc.CreateElement("stf", "DocTypeIndic", "urn:oecd:ties:stf:v4");
            stf_DocTypeIndic.AppendChild(doc.CreateTextNode("OECD1"));
            crs_DocSpec.AppendChild(stf_DocTypeIndic);

            XmlNode stf_DocRefId = doc.CreateElement("stf", "DocRefId", "urn:oecd:ties:stf:v4");
            stf_DocRefId.AppendChild(doc.CreateTextNode(docrefid));
            crs_DocSpec.AppendChild(stf_DocRefId);
            
            crs_ReportingFI.AppendChild(crs_DocSpec);
            /*Children of crs_ReportingFI*/

            crs_CrsBody.AppendChild(crs_ReportingFI);


            XmlNode crs_ReportingGroup = doc.CreateElement("crs", "ReportingGroup", "urn:oecd:ties:crs:v1");

            createCRSTemplate(crs_ReportingGroup, doc, list);

            crs_CrsBody.AppendChild(crs_ReportingGroup);
            


            CRS_OECD.AppendChild(crs_CrsBody);
            /*Children of crs_CrsBody*/

            doc.AppendChild(CRS_OECD);

            doc.Save(Console.Out);
            doc.Save(absolutPath + xmlfilename+".xml");

            return null;
        }
        public static String createXML(string path, List<Variables.FatcaData> list, string absolutPath, 
            string SendingCompanyIN, string docrefid,string companyName, string periodEnding, 
            string MessageRefId, string shortaddress, string xmlfilename)
        {

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode FATCA_OECD = doc.CreateElement("ftc:FATCA_OECD", "FATCA_OECD");
           
           
            /*Attributes*/
            XmlAttribute version = doc.CreateAttribute("version");
            version.Value = "2.0";
            FATCA_OECD.Attributes.Append(version);

            XmlAttribute xmlns_ftc = doc.CreateAttribute("xmlns:ftc");
            xmlns_ftc.Value = "urn:oecd:ties:fatca:v2";
            FATCA_OECD.Attributes.Append(xmlns_ftc);

            XmlAttribute xmlns_sfa = doc.CreateAttribute("xmlns:sfa");
            xmlns_sfa.Value = "urn:oecd:ties:stffatcatypes:v2";
            FATCA_OECD.Attributes.Append(xmlns_sfa);

            XmlAttribute xmlns_xsi = doc.CreateAttribute("xmlns:xsi");
            xmlns_xsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
            FATCA_OECD.Attributes.Append(xmlns_xsi);

            //XmlAttribute xsi_schemaLocation = doc.CreateAttribute("xsi:schemaLocation");
            XmlAttribute xsi_schemaLocation = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsi_schemaLocation.Value = "urn:oecd:ties:fatca:v2 FatcaXML_v2.0.xsd";
            FATCA_OECD.Attributes.Append(xsi_schemaLocation);
            /*Attributes Of FATCA_OECD*/

            XmlNode ftc_MessageSpec = doc.CreateElement("ftc", "MessageSpec", "urn:oecd:ties:fatca:v2");
            /*Children of ftc_MessageSpec*/
            XmlNode sfa_SendingCompanyIN = doc.CreateElement("sfa", "SendingCompanyIN", "urn:oecd:ties:stffatcatypes:v2");
            sfa_SendingCompanyIN.AppendChild(doc.CreateTextNode(SendingCompanyIN));
            //sfa_SendingCompanyIN.AppendChild(doc.CreateTextNode("LFDG90.00000.LE.052"));
            ftc_MessageSpec.AppendChild(sfa_SendingCompanyIN);

            XmlNode sfa_TransmittingCountry = doc.CreateElement("sfa", "TransmittingCountry", "urn:oecd:ties:stffatcatypes:v2");
            sfa_TransmittingCountry.AppendChild(doc.CreateTextNode("BB"));
            ftc_MessageSpec.AppendChild(sfa_TransmittingCountry);

            XmlNode sfa_ReceivingCountry = doc.CreateElement("sfa", "ReceivingCountry", "urn:oecd:ties:stffatcatypes:v2");
            sfa_ReceivingCountry.AppendChild(doc.CreateTextNode("US"));
            ftc_MessageSpec.AppendChild(sfa_ReceivingCountry);

            XmlNode sfa_MessageType = doc.CreateElement("sfa", "MessageType", "urn:oecd:ties:stffatcatypes:v2");
            sfa_MessageType.AppendChild(doc.CreateTextNode("FATCA"));
            ftc_MessageSpec.AppendChild(sfa_MessageType);

            XmlNode sfa_Warning = doc.CreateElement("sfa", "Warning", "urn:oecd:ties:stffatcatypes:v2");
            ftc_MessageSpec.AppendChild(sfa_Warning);

            XmlNode sfa_MessageRefId = doc.CreateElement("sfa", "MessageRefId", "urn:oecd:ties:stffatcatypes:v2");
            sfa_MessageRefId.AppendChild(doc.CreateTextNode(SendingCompanyIN+ "_0227_BPW"));
            ftc_MessageSpec.AppendChild(sfa_MessageRefId);

            XmlNode sfa_ReportingPeriod = doc.CreateElement("sfa", "ReportingPeriod", "urn:oecd:ties:stffatcatypes:v2");
            sfa_ReportingPeriod.AppendChild(doc.CreateTextNode(periodEnding));
            ftc_MessageSpec.AppendChild(sfa_ReportingPeriod);

            XmlNode sfa_Timestamp = doc.CreateElement("sfa", "Timestamp", "urn:oecd:ties:stffatcatypes:v2");
            //2018-07-12
            sfa_Timestamp.AppendChild(doc.CreateTextNode(periodEnding+"T15:36:00"));
            ftc_MessageSpec.AppendChild(sfa_Timestamp);
            /*Children of ftc_MessageSpec*/


            XmlNode ftc_FATCA = doc.CreateElement("ftc", "FATCA", "urn:oecd:ties:fatca:v2");

            /*Children of ftc_FATCA*/
            XmlNode ftc_ReportingFI = doc.CreateElement("ftc", "ReportingFI", "urn:oecd:ties:fatca:v2");
            

            /*Children of ftc_ReportingFI*/
            XmlNode sfa_ResCountryCode = doc.CreateElement("sfa", "ResCountryCode", "urn:oecd:ties:stffatcatypes:v2");
            sfa_ResCountryCode.AppendChild(doc.CreateTextNode("BB"));
            ftc_ReportingFI.AppendChild(sfa_ResCountryCode);

            XmlNode sfa_TIN = doc.CreateElement("sfa", "TIN", "urn:oecd:ties:stffatcatypes:v2");
            sfa_TIN.AppendChild(doc.CreateTextNode(SendingCompanyIN));
            XmlAttribute sfa_TINAttribute = doc.CreateAttribute("issuedBy");
            sfa_TINAttribute.Value = "US";
            sfa_TIN.Attributes.Append(sfa_TINAttribute);
            ftc_ReportingFI.AppendChild(sfa_TIN);

            XmlNode sfa_Name = doc.CreateElement("sfa", "Name", "urn:oecd:ties:stffatcatypes:v2");
            sfa_Name.AppendChild(doc.CreateTextNode(companyName));
            ftc_ReportingFI.AppendChild(sfa_Name);

            XmlNode sfa_Address = doc.CreateElement("sfa", "Address", "urn:oecd:ties:stffatcatypes:v2");
            
            /*Children of sfa_Address*/
            XmlNode sfa_CountryCode = doc.CreateElement("sfa", "CountryCode", "urn:oecd:ties:stffatcatypes:v2");
            sfa_CountryCode.AppendChild(doc.CreateTextNode("BB"));
            sfa_Address.AppendChild(sfa_CountryCode);

            XmlNode sfa_AddressFix = doc.CreateElement("sfa", "AddressFix", "urn:oecd:ties:stffatcatypes:v2");
            /*Children of sfa:AddressFix*/
            XmlNode sfa_Street = doc.CreateElement("sfa", "Street", "urn:oecd:ties:stffatcatypes:v2");
            sfa_Street.AppendChild(doc.CreateTextNode(shortaddress));
            sfa_AddressFix.AppendChild(sfa_Street);

            XmlNode sfa_BuildingIdentifier = doc.CreateElement("sfa", "BuildingIdentifier", "urn:oecd:ties:stffatcatypes:v2");
            sfa_AddressFix.AppendChild(sfa_BuildingIdentifier);

            XmlNode sfa_SuiteIdentifier = doc.CreateElement("sfa", "SuiteIdentifier", "urn:oecd:ties:stffatcatypes:v2");
            sfa_AddressFix.AppendChild(sfa_SuiteIdentifier);

            XmlNode sfa_FloorIdentifier = doc.CreateElement("sfa", "FloorIdentifier", "urn:oecd:ties:stffatcatypes:v2");
            sfa_AddressFix.AppendChild(sfa_FloorIdentifier);

            XmlNode sfa_DistrictName = doc.CreateElement("sfa", "DistrictName", "urn:oecd:ties:stffatcatypes:v2");
            sfa_AddressFix.AppendChild(sfa_DistrictName);

            XmlNode sfa_POB = doc.CreateElement("sfa", "POB", "urn:oecd:ties:stffatcatypes:v2");
            sfa_AddressFix.AppendChild(sfa_POB);

            XmlNode sfa_PostCode = doc.CreateElement("sfa", "PostCode", "urn:oecd:ties:stffatcatypes:v2");
            sfa_AddressFix.AppendChild(sfa_PostCode);

            XmlNode sfa_City = doc.CreateElement("sfa", "City", "urn:oecd:ties:stffatcatypes:v2");
            sfa_City.AppendChild(doc.CreateTextNode("Bridgetown"));
            sfa_AddressFix.AppendChild(sfa_City);

            XmlNode sfa_CountrySubentity = doc.CreateElement("sfa", "CountrySubentity", "urn:oecd:ties:stffatcatypes:v2");
            sfa_AddressFix.AppendChild(sfa_CountrySubentity);
            
            /*Children of sfa:AddressFix*/
            sfa_Address.AppendChild(sfa_AddressFix);

            XmlNode sfa_AddressFree = doc.CreateElement("sfa", "AddressFree", "urn:oecd:ties:stffatcatypes:v2");
            sfa_Address.AppendChild(sfa_AddressFree);
            
            /*Children of sfa_Address*/

            XmlNode ftc_DocSpec = doc.CreateElement("ftc", "DocSpec", "urn:oecd:ties:fatca:v2");
            /*Children of ftc_DocSpec*/
            XmlNode ftc_DocTypeIndic = doc.CreateElement("ftc", "DocTypeIndic", "urn:oecd:ties:fatca:v2");
            ftc_DocTypeIndic.AppendChild(doc.CreateTextNode("FATCA1"));
            ftc_DocSpec.AppendChild(ftc_DocTypeIndic);

            XmlNode ftc_DocRefId = doc.CreateElement("ftc", "DocRefId", "urn:oecd:ties:fatca:v2");
            ftc_DocRefId.AppendChild(doc.CreateTextNode(docrefid));
            ftc_DocSpec.AppendChild(ftc_DocRefId);

            XmlNode ftc_CorrMessageRefId = doc.CreateElement("ftc", "CorrMessageRefId", "urn:oecd:ties:fatca:v2");
            ftc_DocSpec.AppendChild(ftc_CorrMessageRefId);

            XmlNode ftc_CorrDocRefId = doc.CreateElement("ftc", "CorrDocRefId", "urn:oecd:ties:fatca:v2");
            ftc_DocSpec.AppendChild(ftc_CorrDocRefId);

            

            /*Children of ftc_DocSpec*/
            ftc_ReportingFI.AppendChild(sfa_Address);
            XmlNode ftc_FilerCategory = doc.CreateElement("ftc", "FilerCategory", "urn:oecd:ties:stffatcatypes:v2");
            ftc_FilerCategory.AppendChild(doc.CreateTextNode("FATCA603"));
            ftc_ReportingFI.AppendChild(ftc_FilerCategory);
            ftc_ReportingFI.AppendChild(ftc_DocSpec);
            
            //<ftc:FilerCategory>FATCA603</ftc:FilerCategory>


            /*Children of ftc_ReportingFI*/

            XmlNode ftc_ReportingGroup = doc.CreateElement("ftc", "ReportingGroup", "urn:oecd:ties:fatca:v2");
            
            /*Array of AccountReport*/

            createTemplate(ftc_ReportingGroup, doc, list);


            /*Children of ftc_FATCA*/
            ftc_FATCA.AppendChild(ftc_ReportingFI);
            ftc_FATCA.AppendChild(ftc_ReportingGroup);

            FATCA_OECD.AppendChild(ftc_MessageSpec);
            FATCA_OECD.AppendChild(ftc_FATCA);

            doc.AppendChild(FATCA_OECD);
            

            doc.Save(Console.Out);
            doc.Save(absolutPath+ xmlfilename+".xml");

            return null;
        }
        public static string lpad(string n, Int32 len)
        {
            Int32 zeroCount = len - n.Length;
            string result = "";

            for (int i = 1; i <= zeroCount; i++)
            {
                result = result + "0";
            }

            result = result + n;

            return result;

        }

    }
}