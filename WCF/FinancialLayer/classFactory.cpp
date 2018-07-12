#include "stdafx.h"

#include <ql/indexes/ibor/libor.hpp>
#include <ql/indexes/ibor/audlibor.hpp>
#include <ql/indexes/ibor/cadlibor.hpp>
#include <ql/indexes/ibor/chflibor.hpp>
#include <ql/indexes/ibor/dkklibor.hpp>
#include <ql/indexes/ibor/gbplibor.hpp>
#include <ql/indexes/ibor/jpylibor.hpp>
#include <ql/indexes/ibor/nzdlibor.hpp>
#include <ql/indexes/ibor/seklibor.hpp>

#include <ql/indexes/inflation/all.hpp>

#include <boost/any.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/algorithm/string.hpp>
#include <map>

#include "classFactory.h"
#include "QuanlibDepositLiborRates.h"
#include "QuantlibSwapRates.h"
#include "Calendars.h"

using namespace __QLCPP;

#define stringify( name ) # name

// calendar.xml
ObjectFactory<QuantLib::Calendar>::ClassNameFactoryMap ObjectFactory<QuantLib::Calendar>::factoryMap;
void ObjectFactory<QuantLib::Calendar>::InitObjectFactory()
{
	RegisterClass<QuantLib::Argentina>();
	RegisterClass<QuantLib::Australia>();
	RegisterClass<QuantLib::BespokeCalendar>();
	RegisterClass<QuantLib::Brazil>();
	RegisterClass<QuantLib::Brazil, QuantLib::Brazil::Market>(QuantLib::Brazil::Settlement, stringify(Settlement));
	RegisterClass<QuantLib::Brazil, QuantLib::Brazil::Market>(QuantLib::Brazil::Exchange, stringify(Exchange));
	RegisterClass<QuantLib::Canada>();
	RegisterClass<QuantLib::Canada, QuantLib::Canada::Market>(QuantLib::Canada::Settlement, stringify(Settlement));
	RegisterClass<QuantLib::Canada, QuantLib::Canada::Market>(QuantLib::Canada::TSX, stringify(TSX));
	RegisterClass<QuantLib::China>();
	RegisterClass<QuantLib::China, QuantLib::China::Market>(QuantLib::China::SSE, stringify(SSE));
	RegisterClass<QuantLib::CzechRepublic>();
	RegisterClass<QuantLib::CzechRepublic, QuantLib::CzechRepublic::Market>(QuantLib::CzechRepublic::PSE, stringify(PSE));
	RegisterClass<QuantLib::Denmark>();
	RegisterClass<QuantLib::Finland>();
	RegisterClass<__QLCPP::France>();
	RegisterClass<QuantLib::Germany>();
	RegisterClass<QuantLib::Germany, QuantLib::Germany::Market>(QuantLib::Germany::Settlement, stringify(Settlement));
	RegisterClass<QuantLib::Germany, QuantLib::Germany::Market>(QuantLib::Germany::FrankfurtStockExchange, stringify(FrankfurtStockExchange));
	RegisterClass<QuantLib::Germany, QuantLib::Germany::Market>(QuantLib::Germany::Xetra, stringify(Xetra));
	RegisterClass<QuantLib::Germany, QuantLib::Germany::Market>(QuantLib::Germany::Eurex, stringify(Eurex));
	RegisterClass<QuantLib::Germany, QuantLib::Germany::Market>(QuantLib::Germany::Euwax, stringify(Euwax));
	RegisterClass<QuantLib::HongKong>();
	RegisterClass<QuantLib::HongKong, QuantLib::HongKong::Market>(QuantLib::HongKong::HKEx, stringify(HKEx));
	RegisterClass<QuantLib::Hungary>();
	RegisterClass<QuantLib::Iceland>();
	RegisterClass<QuantLib::Iceland, QuantLib::Iceland::Market>(QuantLib::Iceland::ICEX, stringify(ICEX));
	RegisterClass<QuantLib::India>();
	RegisterClass<QuantLib::India, QuantLib::India::Market>(QuantLib::India::NSE, stringify(NSE));
	RegisterClass<QuantLib::Indonesia>();
	RegisterClass<QuantLib::Indonesia, QuantLib::Indonesia::Market>(QuantLib::Indonesia::BEJ, stringify(BEJ));
	RegisterClass<QuantLib::Indonesia, QuantLib::Indonesia::Market>(QuantLib::Indonesia::JSX, stringify(JSX));
	RegisterClass<QuantLib::Indonesia, QuantLib::Indonesia::Market>(QuantLib::Indonesia::IDX, stringify(IDX));
	RegisterClass<QuantLib::Italy>();
	RegisterClass<QuantLib::Italy, QuantLib::Italy::Market>(QuantLib::Italy::Settlement, stringify(Settlement));
	RegisterClass<QuantLib::Italy, QuantLib::Italy::Market>(QuantLib::Italy::Exchange, stringify(Exchange));
	RegisterClass<QuantLib::Japan>();
	RegisterClass<QuantLib::Mexico>();
	RegisterClass<QuantLib::Mexico, QuantLib::Mexico::Market>(QuantLib::Mexico::BMV, stringify(BMV));
	RegisterClass<QuantLib::NewZealand>();
	RegisterClass<QuantLib::Norway>();
	RegisterClass<QuantLib::NullCalendar>();
	RegisterClass<QuantLib::Poland>();
	RegisterClass<QuantLib::Russia>();
	RegisterClass<QuantLib::SaudiArabia>();
	RegisterClass<QuantLib::SaudiArabia, QuantLib::SaudiArabia::Market>(QuantLib::SaudiArabia::Tadawul, stringify(Tadawul));
	RegisterClass<QuantLib::Singapore>();
	RegisterClass<QuantLib::Singapore, QuantLib::Singapore::Market>(QuantLib::Singapore::SGX, stringify(SGX));
	RegisterClass<QuantLib::Slovakia>();
	RegisterClass<QuantLib::Slovakia, QuantLib::Slovakia::Market>(QuantLib::Slovakia::BSSE, stringify(BSSE));
	RegisterClass<QuantLib::SouthAfrica>();
	RegisterClass<QuantLib::SouthKorea>();
	RegisterClass<QuantLib::SouthKorea, QuantLib::SouthKorea::Market>(QuantLib::SouthKorea::KRX, stringify(KRX));
	RegisterClass<QuantLib::Sweden>();
	RegisterClass<QuantLib::Switzerland>();
	RegisterClass<QuantLib::Taiwan>();
	RegisterClass<QuantLib::Taiwan, QuantLib::Taiwan::Market>(QuantLib::Taiwan::TSEC, stringify(TSEC));
	RegisterClass<QuantLib::TARGET>();
	RegisterClass<QuantLib::Turkey>();
	RegisterClass<QuantLib::Ukraine>();
	RegisterClass<QuantLib::Ukraine, QuantLib::Ukraine::Market>(QuantLib::Ukraine::USE, stringify(USE));
	RegisterClass<QuantLib::UnitedKingdom>();
	RegisterClass<QuantLib::UnitedKingdom, QuantLib::UnitedKingdom::Market>(QuantLib::UnitedKingdom::Settlement, stringify(Settlement));
	RegisterClass<QuantLib::UnitedKingdom, QuantLib::UnitedKingdom::Market>(QuantLib::UnitedKingdom::Exchange, stringify(Exchange));
	RegisterClass<QuantLib::UnitedKingdom, QuantLib::UnitedKingdom::Market>(QuantLib::UnitedKingdom::Metals, stringify(Metals));
	RegisterClass<QuantLib::UnitedStates>();
	RegisterClass<QuantLib::UnitedStates, QuantLib::UnitedStates::Market>(QuantLib::UnitedStates::Settlement, stringify(Settlement));
	RegisterClass<QuantLib::UnitedStates, QuantLib::UnitedStates::Market>(QuantLib::UnitedStates::NYSE, stringify(NYSE));
	RegisterClass<QuantLib::UnitedStates, QuantLib::UnitedStates::Market>(QuantLib::UnitedStates::GovernmentBond, stringify(GovernmentBond));
	RegisterClass<QuantLib::UnitedStates, QuantLib::UnitedStates::Market>(QuantLib::UnitedStates::NERC, stringify(NERC));
	RegisterClass<QuantLib::WeekendsOnly>();
}

// enumbasis.xml
ObjectFactory<QuantLib::DayCounter>::ClassNameFactoryMap ObjectFactory<QuantLib::DayCounter>::factoryMap;
void ObjectFactory<QuantLib::DayCounter>::InitObjectFactory()
{
	RegisterClass<QuantLib::Actual360>();
	RegisterClass<QuantLib::Actual365Fixed>();
	RegisterClass<QuantLib::ActualActual>();
	RegisterClass<QuantLib::ActualActual, QuantLib::ActualActual::Convention>(QuantLib::ActualActual::ISMA, stringify(ISMA));
	RegisterClass<QuantLib::ActualActual, QuantLib::ActualActual::Convention>(QuantLib::ActualActual::Bond, stringify(Bond));
	RegisterClass<QuantLib::ActualActual, QuantLib::ActualActual::Convention>(QuantLib::ActualActual::ISDA, stringify(ISDA));
	RegisterClass<QuantLib::ActualActual, QuantLib::ActualActual::Convention>(QuantLib::ActualActual::Historical, stringify(Historical));
	RegisterClass<QuantLib::ActualActual, QuantLib::ActualActual::Convention>(QuantLib::ActualActual::Actual365, stringify(Actual365));
	RegisterClass<QuantLib::ActualActual, QuantLib::ActualActual::Convention>(QuantLib::ActualActual::AFB, stringify(AFB));
	RegisterClass<QuantLib::ActualActual, QuantLib::ActualActual::Convention>(QuantLib::ActualActual::Euro, stringify(Euro));
	RegisterClass<QuantLib::Business252>();
	RegisterClass<QuantLib::OneDayCounter>();
	RegisterClass<QuantLib::Thirty360>();
	RegisterClass<QuantLib::Thirty360, QuantLib::Thirty360::Convention>(QuantLib::Thirty360::USA, stringify(USA));
	RegisterClass<QuantLib::Thirty360, QuantLib::Thirty360::Convention>(QuantLib::Thirty360::BondBasis, stringify(BondBasis));
	RegisterClass<QuantLib::Thirty360, QuantLib::Thirty360::Convention>(QuantLib::Thirty360::European, stringify(European));
	RegisterClass<QuantLib::Thirty360, QuantLib::Thirty360::Convention>(QuantLib::Thirty360::EurobondBasis, stringify(EurobondBasis));
	RegisterClass<QuantLib::Thirty360, QuantLib::Thirty360::Convention>(QuantLib::Thirty360::Italian, stringify(Italian));
	
}

// currency.xml
ObjectFactory<QuantLib::Currency>::ClassNameFactoryMap ObjectFactory<QuantLib::Currency>::factoryMap;
void ObjectFactory<QuantLib::Currency>::InitObjectFactory()
{
	RegisterClass<QuantLib::ARSCurrency>();
	RegisterClass<QuantLib::ATSCurrency>(); 
	RegisterClass<QuantLib::AUDCurrency>();
	RegisterClass<QuantLib::BDTCurrency>(); 
	RegisterClass<QuantLib::BEFCurrency>();
	RegisterClass<QuantLib::BGLCurrency>();
	RegisterClass<QuantLib::BRLCurrency>();
	RegisterClass<QuantLib::BYRCurrency>();
	RegisterClass<QuantLib::CADCurrency>();
	RegisterClass<QuantLib::CHFCurrency>();
	RegisterClass<QuantLib::CLPCurrency>();
	RegisterClass<QuantLib::CNYCurrency>();
	RegisterClass<QuantLib::COPCurrency>();
	RegisterClass<QuantLib::CYPCurrency>();
	RegisterClass<QuantLib::CZKCurrency>();
	RegisterClass<QuantLib::DEMCurrency>();
	RegisterClass<QuantLib::DKKCurrency>();
	RegisterClass<QuantLib::EEKCurrency>();
	RegisterClass<QuantLib::ESPCurrency>();
	RegisterClass<QuantLib::EURCurrency>();
	RegisterClass<QuantLib::FIMCurrency>();
	RegisterClass<QuantLib::FRFCurrency>();
	RegisterClass<QuantLib::GBPCurrency>();
	RegisterClass<QuantLib::GRDCurrency>();
	RegisterClass<QuantLib::HKDCurrency>();
	RegisterClass<QuantLib::HUFCurrency>();
	RegisterClass<QuantLib::IEPCurrency>();
	RegisterClass<QuantLib::ILSCurrency>();
	RegisterClass<QuantLib::INRCurrency>();
	RegisterClass<QuantLib::IQDCurrency>();
	RegisterClass<QuantLib::IRRCurrency>();
	RegisterClass<QuantLib::ISKCurrency>();
	RegisterClass<QuantLib::ITLCurrency>();
	RegisterClass<QuantLib::JPYCurrency>();
	RegisterClass<QuantLib::KRWCurrency>();
	RegisterClass<QuantLib::KWDCurrency>();
	RegisterClass<QuantLib::LTLCurrency>();
	RegisterClass<QuantLib::LUFCurrency>();
	RegisterClass<QuantLib::LVLCurrency>();
	RegisterClass<QuantLib::MTLCurrency>();
	RegisterClass<QuantLib::MXNCurrency>();
	RegisterClass<QuantLib::NLGCurrency>();
	RegisterClass<QuantLib::NOKCurrency>();
	RegisterClass<QuantLib::NPRCurrency>();
	RegisterClass<QuantLib::NZDCurrency>();
	RegisterClass<QuantLib::PEHCurrency>();
	RegisterClass<QuantLib::PEICurrency>();
	RegisterClass<QuantLib::PENCurrency>();
	RegisterClass<QuantLib::PKRCurrency>();
	RegisterClass<QuantLib::PLNCurrency>();
	RegisterClass<QuantLib::PTECurrency>();
	RegisterClass<QuantLib::ROLCurrency>();
	RegisterClass<QuantLib::RONCurrency>();
	RegisterClass<QuantLib::SARCurrency>();
	RegisterClass<QuantLib::SEKCurrency>();
	RegisterClass<QuantLib::SGDCurrency>();
	RegisterClass<QuantLib::SITCurrency>();
	RegisterClass<QuantLib::SKKCurrency>();
	RegisterClass<QuantLib::THBCurrency>();
	RegisterClass<QuantLib::TRLCurrency>();
	RegisterClass<QuantLib::TRYCurrency>();
	RegisterClass<QuantLib::TTDCurrency>();
	RegisterClass<QuantLib::TWDCurrency>();
	RegisterClass<QuantLib::USDCurrency>();
	RegisterClass<QuantLib::VEBCurrency>();
	RegisterClass<QuantLib::ZARCurrency>();
}

// rate.xml
ObjectFactory<QuantLib::IborIndex>::ClassNameFactoryMap ObjectFactory<QuantLib::IborIndex>::factoryMap;
void ObjectFactory<QuantLib::IborIndex>::InitObjectFactory()
{
	RegisterClass<QuantLib::EURLiborON>();
	RegisterClass<QuantLib::EURLiborSW>();
	RegisterClass<QuantLib::EURLibor2W>();
	RegisterClass<QuantLib::EURLibor1M>();
	RegisterClass<QuantLib::EURLibor2M>();
	RegisterClass<QuantLib::EURLibor3M>();
	RegisterClass<QuantLib::EURLibor4M>();
	RegisterClass<QuantLib::EURLibor5M>();
	RegisterClass<QuantLib::EURLibor6M>();
	RegisterClass<QuantLib::EURLibor7M>();
	RegisterClass<QuantLib::EURLibor8M>();
	RegisterClass<QuantLib::EURLibor9M>();
	RegisterClass<QuantLib::EURLibor10M>();
	RegisterClass<QuantLib::EURLibor11M>();
	RegisterClass<QuantLib::EURLibor1Y>();
	RegisterClass<QuantLib::EuriborSW>();
	RegisterClass<QuantLib::Euribor2W>();
	RegisterClass<QuantLib::Euribor3W>();
	RegisterClass<QuantLib::Euribor1M>();
	RegisterClass<QuantLib::Euribor2M>();
	RegisterClass<QuantLib::Euribor3M>();
	RegisterClass<QuantLib::Euribor4M>();
	RegisterClass<QuantLib::Euribor5M>();
	RegisterClass<QuantLib::Euribor6M>();
	RegisterClass<QuantLib::Euribor7M>();
	RegisterClass<QuantLib::Euribor8M>();
	RegisterClass<QuantLib::Euribor9M>();
	RegisterClass<QuantLib::Euribor10M>();
	RegisterClass<QuantLib::Euribor11M>();
	RegisterClass<QuantLib::Euribor1Y>();
	RegisterClass<QuantLib::Euribor365_SW>();
	RegisterClass<QuantLib::Euribor365_2W>();
	RegisterClass<QuantLib::Euribor365_3W>();
	RegisterClass<QuantLib::Euribor365_1M>();
	RegisterClass<QuantLib::Euribor365_1Y>();
	RegisterClass<QuantLib::Euribor365_2M>();
	RegisterClass<QuantLib::Euribor365_3M>();
	RegisterClass<QuantLib::Euribor365_4M>();
	RegisterClass<QuantLib::Euribor365_5M>();
	RegisterClass<QuantLib::Euribor365_6M>();
	RegisterClass<QuantLib::Euribor365_7M>();
	RegisterClass<QuantLib::Euribor365_8M>();
	RegisterClass<QuantLib::Euribor365_9M>();
	RegisterClass<QuantLib::Euribor365_10M>();
	RegisterClass<QuantLib::Euribor365_11M>();
	RegisterClass<QuantLib::USDLiborON>();
	RegisterClass<QuantLib::USDLiborSW>();
	RegisterClass<QuantLib::USDLibor2W>();
	RegisterClass<QuantLib::USDLibor1M>();
	RegisterClass<QuantLib::USDLibor2M>();
	RegisterClass<QuantLib::USDLibor3M>();
	RegisterClass<QuantLib::USDLibor4M>();
	RegisterClass<QuantLib::USDLibor5M>();
	RegisterClass<QuantLib::USDLibor6M>();
	RegisterClass<QuantLib::USDLibor7M>();
	RegisterClass<QuantLib::USDLibor8M>();
	RegisterClass<QuantLib::USDLibor9M>();
	RegisterClass<QuantLib::USDLibor10M>();
	RegisterClass<QuantLib::USDLibor11M>();
	RegisterClass<QuantLib::USDLibor1Y>();
	RegisterClass<QuantLib::USDLiborON>();
	RegisterClass<QuantLib::GBPLiborON>();
	RegisterClass<QuantLib::GBPLiborSW>();
	RegisterClass<QuantLib::GBPLibor2W>();
	RegisterClass<QuantLib::GBPLibor1M>();
	RegisterClass<QuantLib::GBPLibor2M>();
	RegisterClass<QuantLib::GBPLibor3M>();
	RegisterClass<QuantLib::GBPLibor4M>();
	RegisterClass<QuantLib::GBPLibor5M>();
	RegisterClass<QuantLib::GBPLibor6M>();
	RegisterClass<QuantLib::GBPLibor7M>();
	RegisterClass<QuantLib::GBPLibor8M>();
	RegisterClass<QuantLib::GBPLibor9M>();
	RegisterClass<QuantLib::GBPLibor10M>();
	RegisterClass<QuantLib::GBPLibor11M>();
	RegisterClass<QuantLib::GBPLibor1Y>();
	RegisterClass<QuantLib::AUDLiborON>();
	RegisterClass<QuantLib::AUDLiborSW>();
	RegisterClass<QuantLib::AUDLibor2W>();
	RegisterClass<QuantLib::AUDLibor1M>();
	RegisterClass<QuantLib::AUDLibor2M>();
	RegisterClass<QuantLib::AUDLibor3M>();
	RegisterClass<QuantLib::AUDLibor4M>();
	RegisterClass<QuantLib::AUDLibor5M>();
	RegisterClass<QuantLib::AUDLibor6M>();
	RegisterClass<QuantLib::AUDLibor7M>();
	RegisterClass<QuantLib::AUDLibor8M>();
	RegisterClass<QuantLib::AUDLibor9M>();
	RegisterClass<QuantLib::AUDLibor10M>();
	RegisterClass<QuantLib::AUDLibor11M>();
	RegisterClass<QuantLib::AUDLibor1Y>();
	RegisterClass<QuantLib::CADLiborON>();
	RegisterClass<QuantLib::CADLiborSW>();
	RegisterClass<QuantLib::CADLibor2W>();
	RegisterClass<QuantLib::CADLibor1M>();
	RegisterClass<QuantLib::CADLibor2M>();
	RegisterClass<QuantLib::CADLibor3M>();
	RegisterClass<QuantLib::CADLibor4M>();
	RegisterClass<QuantLib::CADLibor5M>();
	RegisterClass<QuantLib::CADLibor6M>();
	RegisterClass<QuantLib::CADLibor7M>();
	RegisterClass<QuantLib::CADLibor8M>();
	RegisterClass<QuantLib::CADLibor9M>();
	RegisterClass<QuantLib::CADLibor10M>();
	RegisterClass<QuantLib::CADLibor11M>();
	RegisterClass<QuantLib::CADLibor1Y>();
	RegisterClass<QuantLib::CHFLiborON>();
	RegisterClass<QuantLib::CHFLiborSW>();
	RegisterClass<QuantLib::CHFLibor2W>();
	RegisterClass<QuantLib::CHFLibor1M>();
	RegisterClass<QuantLib::CHFLibor2M>();
	RegisterClass<QuantLib::CHFLibor3M>();
	RegisterClass<QuantLib::CHFLibor4M>();
	RegisterClass<QuantLib::CHFLibor5M>();
	RegisterClass<QuantLib::CHFLibor6M>();
	RegisterClass<QuantLib::CHFLibor7M>();
	RegisterClass<QuantLib::CHFLibor8M>();
	RegisterClass<QuantLib::CHFLibor9M>();
	RegisterClass<QuantLib::CHFLibor10M>();
	RegisterClass<QuantLib::CHFLibor11M>();
	RegisterClass<QuantLib::CHFLibor1Y>();
	RegisterClass<QuantLib::DKKLiborON>();
	RegisterClass<QuantLib::DKKLiborSW>();
	RegisterClass<QuantLib::DKKLibor2W>();
	RegisterClass<QuantLib::DKKLibor1M>();
	RegisterClass<QuantLib::DKKLibor2M>();
	RegisterClass<QuantLib::DKKLibor3M>();
	RegisterClass<QuantLib::DKKLibor4M>();
	RegisterClass<QuantLib::DKKLibor5M>();
	RegisterClass<QuantLib::DKKLibor6M>();
	RegisterClass<QuantLib::DKKLibor7M>();
	RegisterClass<QuantLib::DKKLibor8M>();
	RegisterClass<QuantLib::DKKLibor9M>();
	RegisterClass<QuantLib::DKKLibor10M>();
	RegisterClass<QuantLib::DKKLibor11M>();
	RegisterClass<QuantLib::DKKLibor1Y>();
	RegisterClass<QuantLib::JPYLiborON>();
	RegisterClass<QuantLib::JPYLiborSW>();
	RegisterClass<QuantLib::JPYLibor2W>();
	RegisterClass<QuantLib::JPYLibor1M>();
	RegisterClass<QuantLib::JPYLibor2M>();
	RegisterClass<QuantLib::JPYLibor3M>();
	RegisterClass<QuantLib::JPYLibor4M>();
	RegisterClass<QuantLib::JPYLibor5M>();
	RegisterClass<QuantLib::JPYLibor6M>();
	RegisterClass<QuantLib::JPYLibor7M>();
	RegisterClass<QuantLib::JPYLibor8M>();
	RegisterClass<QuantLib::JPYLibor9M>();
	RegisterClass<QuantLib::JPYLibor10M>();
	RegisterClass<QuantLib::JPYLibor11M>();
	RegisterClass<QuantLib::JPYLibor1Y>();
	RegisterClass<QuantLib::NZDLiborON>();
	RegisterClass<QuantLib::NZDLiborSW>();
	RegisterClass<QuantLib::NZDLibor2W>();
	RegisterClass<QuantLib::NZDLibor1M>();
	RegisterClass<QuantLib::NZDLibor2M>();
	RegisterClass<QuantLib::NZDLibor3M>();
	RegisterClass<QuantLib::NZDLibor4M>();
	RegisterClass<QuantLib::NZDLibor5M>();
	RegisterClass<QuantLib::NZDLibor6M>();
	RegisterClass<QuantLib::NZDLibor7M>();
	RegisterClass<QuantLib::NZDLibor8M>();
	RegisterClass<QuantLib::NZDLibor9M>();
	RegisterClass<QuantLib::NZDLibor10M>();
	RegisterClass<QuantLib::NZDLibor11M>();
	RegisterClass<QuantLib::NZDLibor1Y>();
	RegisterClass<QuantLib::SEKLiborON>();
	RegisterClass<QuantLib::SEKLiborSW>();
	RegisterClass<QuantLib::SEKLibor2W>();
	RegisterClass<QuantLib::SEKLibor1M>();
	RegisterClass<QuantLib::SEKLibor2M>();
	RegisterClass<QuantLib::SEKLibor3M>();
	RegisterClass<QuantLib::SEKLibor4M>();
	RegisterClass<QuantLib::SEKLibor5M>();
	RegisterClass<QuantLib::SEKLibor6M>();
	RegisterClass<QuantLib::SEKLibor7M>();
	RegisterClass<QuantLib::SEKLibor8M>();
	RegisterClass<QuantLib::SEKLibor9M>();
	RegisterClass<QuantLib::SEKLibor10M>();
	RegisterClass<QuantLib::SEKLibor11M>();
	RegisterClass<QuantLib::SEKLibor1Y>();
}

// rate.xml
ObjectFactory<QuantLib::SwapIndex>::ClassNameFactoryMap ObjectFactory<QuantLib::SwapIndex>::factoryMap;
void ObjectFactory<QuantLib::SwapIndex>::InitObjectFactory()
{
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA1y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA2y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA3y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA4y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA5y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA6y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA7y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA8y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA9y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA10y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA12y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA15y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA20y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA25y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixA30y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB1y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB2y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB3y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB4y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB5y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB6y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB7y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB8y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB9y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB10y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB12y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB15y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB20y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB25y>();
	RegisterClass<QuantLib::EurLiborSwapIsdaFixB30y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix1y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix2y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix3y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix4y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix5y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix6y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix7y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix8y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix9y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix10y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix12y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix15y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix20y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix25y>();
	RegisterClass<QuantLib::EurLiborSwapIfrFix30y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA1y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA2y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA3y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA4y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA5y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA6y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA7y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA8y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA9y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA10y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA12y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA15y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA20y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA25y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixA30y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB1y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB2y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB3y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB4y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB5y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB6y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB7y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB8y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB9y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB10y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB12y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB15y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB20y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB25y>();
	RegisterClass<QuantLib::EuriborSwapIsdaFixB30y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix1y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix2y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix3y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix4y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix5y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix6y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix7y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix8y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix9y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix10y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix12y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix15y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix20y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix25y>();
	RegisterClass<QuantLib::GbpLiborSwapIsdaFix30y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm1y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm2y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm3y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm4y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm5y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm6y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm7y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm8y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm9y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm10y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm15y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm20y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixAm30y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm1y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm2y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm3y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm4y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm5y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm6y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm7y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm8y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm9y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm10y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm15y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm20y>();
	RegisterClass<QuantLib::UsdLiborSwapIsdaFixPm30y>(); 
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm1y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm2y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm3y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm4y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm5y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm6y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm7y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm8y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm9y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm10y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm12y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm15y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm20y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm25y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm30y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm35y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm40y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm1y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixAm40y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm1y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm2y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm3y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm4y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm5y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm6y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm7y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm8y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm9y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm10y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm12y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm15y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm20y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm25y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm30y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm35y>();
	RegisterClass<QuantLib::JpyLiborSwapIsdaFixPm40y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix1y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix2y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix3y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix4y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix5y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix6y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix7y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix8y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix9y>();
	RegisterClass<QuantLib::ChfLiborSwapIsdaFix10y>(); 
}



//---------------------------- Inflation -----------------------------

ObjectFactory<QuantLib::InflationIndex>::ClassNameFactoryMap ObjectFactory<QuantLib::InflationIndex>::factoryMap;
void ObjectFactory<QuantLib::InflationIndex>::InitObjectFactory()
{
	//RegisterClass<QuantLib::UKRPI>(new QuantLib::UKRPI(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	//RegisterClass<QuantLib::AUCPI>(new QuantLib::AUCPI(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	//RegisterClass<QuantLib::EUHICP>(new QuantLib::EUHICP(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	//RegisterClass<QuantLib::EUHICPXT>(new QuantLib::EUHICPXT(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	//RegisterClass<QuantLib::FRHICP>(new QuantLib::FRHICP(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	//RegisterClass<QuantLib::GenericCPI>();
	//RegisterClass<QuantLib::USCPI>(new QuantLib::USCPI(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	//RegisterClass<QuantLib::YYAUCPI>(new QuantLib::YYAUCPI(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	//RegisterClass<QuantLib::YYAUCPIr>(new QuantLib::YYAUCPIr(false, QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure>));
	
}

boost::shared_ptr<QuantLib::ZeroInflationIndex> 
	ObjectFactory<QuantLib::ZeroInflationIndex>::CreateInflationIndexFactory(std::string& className)
{
	bool interp = false;
	QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure> hz;

	boost::shared_ptr<QuantLib::ZeroInflationIndex> iiCPI;

	if (boost::iequals(className, std::string("UKRPI")))
	{
		boost::shared_ptr<QuantLib::UKRPI> iiUKRPI(new QuantLib::UKRPI(interp, hz));
		iiCPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUKRPI);
	}
	else if (boost::iequals(className, std::string("USCPI")))
	{
		boost::shared_ptr<QuantLib::USCPI> iiUSCPI(new QuantLib::USCPI(interp, hz));
		iiCPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUSCPI);
	}
	else if (boost::iequals(className, std::string("GermanCPI")))
	{
		/* -- app domain problem due to having static local variable in GenericRegion CTOR:
		check: http://social.msdn.microsoft.com/Forums/en/clr/thread/cd3c34a6-84f9-4e2b-a483-12e179eb84b7
					
		GenericRegion() {
				static boost::shared_ptr<Data> GENERICdata(
                                               new Data("Generic","GENERIC"));
				data_ = GENERICdata;
			}

			-- to workaround, use:
			#pragma managed(push, off)
			#pragma managed (pop)
			*/
		bool revised = false;

		boost::shared_ptr<QuantLib::GenericCPI> iigeneric (
			new QuantLib::GenericCPI(
					QuantLib::Frequency::Monthly, 
					revised, 
					interp, 
					QuantLib::Period(1, QuantLib::Months), 
					QuantLib::EURCurrency(), 
					hz
				)
			);

		iiCPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iigeneric);
	}
	else if (boost::iequals(className, std::string("ITCPI")))
	{
		bool revised = false;

		boost::shared_ptr<QuantLib::GenericCPI> iigeneric (
			new QuantLib::GenericCPI(
					QuantLib::Frequency::Monthly, 
					revised,  
					interp,
					QuantLib::Period(1, QuantLib::Months),
					QuantLib::EURCurrency(),  
					hz
				)
			);

		iiCPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iigeneric);
	}
	else if (boost::iequals(className, std::string("CPTFEMU Index")))
	{
		/* -- app domain problem due to having static local variable in GenericRegion CTOR:
		check: http://social.msdn.microsoft.com/Forums/en/clr/thread/cd3c34a6-84f9-4e2b-a483-12e179eb84b7
					
		GenericRegion() {
				static boost::shared_ptr<Data> GENERICdata(
                                               new Data("Generic","GENERIC"));
				data_ = GENERICdata;
			}

			-- to workaround, use:
			#pragma managed(push, off)
			#pragma managed (pop)
			*/
		bool revised = false;

		boost::shared_ptr<QuantLib::GenericCPI> iigeneric (
			new QuantLib::GenericCPI(
					QuantLib::Frequency::Monthly, 
					revised, 
					interp, 
					QuantLib::Period(1, QuantLib::Months), 
					QuantLib::EURCurrency(), 
					hz
				)
			);

		iiCPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iigeneric);
	}

	return iiCPI;
}