//#pragma managed(push, off)

//#pragma managed(pop)

#using <System.dll>
#using <System.Data.dll>
#using <System.Xml.dll>
#using "Utilities.dll"
#using "DataLayer.dll"

#include "StdAfx.h"
#include <iostream>

#include <boost/any.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/algorithm/string.hpp>
#include <map>
#include <ql/timeseries.hpp>

#include "QuantlibCustomRate.h"
#include "QuantlibSwapRates.h"
#include "classFactory.h"

#define LENGTH(a) (sizeof(a)/sizeof(a[0]))

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace DataLayer;

//#include "FinancialLayer.h"

void MarshalString ( String ^ s, std::string& os ) 
{
	using namespace Runtime::InteropServices;
	const char* chars = 
		(const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	os = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}

#define OBJECT_FACTORY(VAR, BASE_NAME, CLASS_NAME) \
{\
	try\
	{\
		std::string __tmp;\
		MarshalString(CLASS_NAME, __tmp);\
		VAR = static_cast<BASE_NAME*>(IntPtr(__QLCPP::ObjectFactory<BASE_NAME>::ObjectCreator(__tmp)).ToPointer());\
	}\
	catch (...)\
	{\
		throw gcnew Exception("ObjectFactory exception on: " + CLASS_NAME);\
	}\
}
/*
#define OBJECT_FACTORY(VAR, BASE_NAME, CLASS_NAME) \
{\
	try\
	{\
		std::string tmp;\
		MarshalString(CLASS_NAME, tmp);\
		void* ptr = __QLCPP::ObjectFactory2::ObjectCreator(tmp);\
		VAR = static_cast<BASE_NAME*>(IntPtr(ptr).ToPointer());\
	}\
	catch (...)\
	{\
		throw gcnew Exception("ObjectFactory exception on: " + CLASS_NAME);\
	}\
}
*/
struct null_deleter // Does nothing
{
    void operator()(void const*) const {}
};

namespace FinancialLayer 
{
	public ref class QuantLibAdaptor
	{
	public:
		static void Init();
		static void CalculateDiscountedRate(YieldCurveData^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result);
		static void CalculateInflationRate(InflationCurveSnapshot^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result);
		static void CalculateInflationRate1(InflationCurveSnapshot^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result);
		static void CalculateInflationRate1a(InflationCurveSnapshot^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result);
		static void CalculateInflationRate1b(InflationCurveSnapshot^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result);
		static void CalculateInflationRate2(InflationCurveSnapshot^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result);
		static void IterativeInflationCurveCalculation(InflationCurveSnapshot^ ics, 
		List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result);
		static void GetInstrumentMaturity(List<Instrument^>^ instruments, DateTime^ d, List<DateTime>^ datesToDiscount); //, DateTime^ d, List<DateTime^>^ result
		
		static bool GetRateInformation(DataLayer::Rate^ rate);
		static bool GetCurrencyInformation(DataLayer::Currency^ c);
		static bool QuantLibAdaptor::GetDayCounterInformation(DataLayer::DayCounter^ d);
		
	private:
		static void CalculateDiscountedRateMain(
							DateTime^ dd0,									// the 1st date from date to discount list to check each point (if dd0 is provided)
							QuantLib::DayCounter*& termStructureDayCounter,	// has the meaning along with 1st parameter
							double& tolerance,								// has the meaning along with 1st parameter
							YieldCurveData^ ycd, 
							QuantLib::Date& settlementDate, 
							std::vector<boost::shared_ptr<QuantLib::RateHelper> >& depoSwapInstruments );

		static std::string ConvertToStdString(String^ s);
		static QuantLib::TimeUnit ConvertToTimeUnit(String^ s);
		static QuantLib::Compounding ConvertToCompounding(String^ s);
		static QuantLib::BusinessDayConvention ConvertToBusinessDayConvention(String^ s);
		static QuantLib::Frequency ConvertToFrequency(String^ s);
		static QuantLib::Calendar ConvertToCalendar(DataLayer::Calendar^ calendar);
	};

	void QuantLibAdaptor::Init()
	{
		__QLCPP::ObjectFactory<QuantLib::Calendar>::InitObjectFactory();
		__QLCPP::ObjectFactory<QuantLib::DayCounter>::InitObjectFactory();
		__QLCPP::ObjectFactory<QuantLib::Currency>::InitObjectFactory();
		__QLCPP::ObjectFactory<QuantLib::IborIndex>::InitObjectFactory();
		__QLCPP::ObjectFactory<QuantLib::SwapIndex>::InitObjectFactory();
		
		//__QLCPP::ObjectFactory2::InitObjectFactory();

		//
		// Check integraty
		//

		// 1. DayCounter

		List<DayCounter^> dayCounterList = Repository::GetDayCounters();
		for each (DayCounter^ dc in dayCounterList)
		{
			QuantLib::DayCounter* tmp = NULL;
			OBJECT_FACTORY(tmp, QuantLib::DayCounter, dc->ClassName)
		}
		//
	}

	void QuantLibAdaptor::CalculateDiscountedRateMain(
		DateTime^ dd0,									// the 1st date from date to discount list to check each point (if dd0 is provided)
		QuantLib::DayCounter*& termStructureDayCounter,	// has the meaning along with 1st parameter
		double& tolerance,								// has the meaning along with 1st parameter
		YieldCurveData^ ycd, 
		QuantLib::Date& settlementDate, 
		std::vector<boost::shared_ptr<QuantLib::RateHelper> >& depoSwapInstruments )
	{
		depoSwapInstruments.clear();

		for (int i = 0; i < ycd->entryPointList->Count; i++ )
		{
			EntryPoint^ yc = (ycd->entryPointList)[i];
			
			if (yc == nullptr || yc->epValue == nullptr)
				continue;

			QuantLib::Calendar* calendar = NULL;
			
			if (!String::IsNullOrEmpty(ycd->settings->Calendar->ClassName))
			{
				if (!String::IsNullOrEmpty(ycd->settings->Calendar->MarketName)){
					OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName+"::"+ycd->settings->Calendar->MarketName));}
				else
					OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName));
			}
			else
				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String("UnitedKingdom"));
			
  			settlementDate = calendar->adjust(settlementDate);	// must be a business day

			double val = yc->epValue->Value;

			if (0 == String::Compare(yc->Type, gcnew String("bond"), StringComparison::OrdinalIgnoreCase))
				val *= 100; 
				
			boost::shared_ptr<QuantLib::Quote> rate (new QuantLib::SimpleQuote((QuantLib::Rate)val));

			// epValue : type deposit
			if (0 == String::Compare(yc->Type, gcnew String("deposit"), StringComparison::OrdinalIgnoreCase))
			{
				Rate^ _rate = dynamic_cast<Rate^>(yc->Instrument);

				if (!String::IsNullOrEmpty(_rate->ClassName))
				{
					QuantLib::IborIndex* dIndex = NULL;
					OBJECT_FACTORY(dIndex, QuantLib::IborIndex, _rate->ClassName)
					
					boost::shared_ptr<QuantLib::IborIndex> depositIndex(dIndex, null_deleter());

			/*		QuantLib::TimeSeries<QuantLib::Real> ts;
					ts[QuantLib::Date(17, QuantLib::August, 2012)] = val;
					depositIndex->addFixings(ts); */

					boost::shared_ptr<QuantLib::RateHelper> tmp (
						new QuantLib::DepositRateHelper(QuantLib::Handle<QuantLib::Quote>(rate), depositIndex));
					
					depoSwapInstruments.push_back(tmp);
				}
				else //custom deposit rates passed from the database by parameters
				{
					QuantLib::DayCounter* basis = NULL;
					OBJECT_FACTORY(basis, QuantLib::DayCounter, Repository::DayCounterDic[_rate->BasisId]->ClassName)
					
					boost::shared_ptr<QuantLib::RateHelper> tmp (
						new QuantLib::DepositRateHelper(
														QuantLib::Handle<QuantLib::Quote>(rate),
														yc->Length * ConvertToTimeUnit(yc->TimeUnit), 
														_rate->SettlementDays,
														*calendar, 
														QuantLib::ModifiedFollowing,
														true, 
														*basis
														)
					);

					depoSwapInstruments.push_back(tmp);
				}
			}
			// epValue : type swap
			else if (0 == String::Compare(yc->Type, gcnew String("swap"), StringComparison::OrdinalIgnoreCase))
			{
				Rate^ _rate = dynamic_cast<Rate^>(yc->Instrument);

				if (!String::IsNullOrEmpty(_rate->ClassName))  //ready swap rate classes from Quantlib passed by name
				{
					QuantLib::SwapIndex* swIndex = NULL;
					OBJECT_FACTORY(swIndex, QuantLib::SwapIndex, _rate->ClassName)
					
					boost::shared_ptr<QuantLib::SwapIndex> swapIndex(swIndex, null_deleter());

					boost::shared_ptr<QuantLib::RateHelper> tmp (
						new QuantLib::SwapRateHelper(QuantLib::Handle<QuantLib::Quote>(rate), swapIndex)
						);

					depoSwapInstruments.push_back(tmp);
				}
				else //custom swap rate helpers passed by all parameters
				{
					//add here the source code for custom swap rate
				}
			}
			// epValue : type bond
			else if (0 == String::Compare(yc->Type, gcnew String("bond"), StringComparison::OrdinalIgnoreCase))
			{
				Bond^ _bond = dynamic_cast<Bond^>(yc->Instrument);

				std::vector<QuantLib::Rate> coupons(1, _bond->Coupon);
					
				QuantLib::DayCounter* bondDayCounter = NULL;
				OBJECT_FACTORY(bondDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[_bond->CouponBasisId]->ClassName)
					
				QuantLib::BusinessDayConvention bondConvention = ConvertToBusinessDayConvention(_bond->CouponConvention);
				DateTime^ issue = _bond->IssueDate;
				QuantLib::Date issueDate(issue->Day, (QuantLib::Month)(issue->Month), issue->Year);
				DateTime^ maturity = _bond->MaturityDate;
				QuantLib::Date maturityDate(maturity->Day, (QuantLib::Month)(maturity->Month), maturity->Year);
				QuantLib::Frequency bondFreq = ConvertToFrequency(_bond->CouponFrequency);

				QuantLib::Schedule sch (QuantLib::Schedule(issueDate, maturityDate,QuantLib::Period(bondFreq), 
                                        *calendar, bondConvention, bondConvention,
                                        QuantLib::DateGeneration::Backward, false)); 

				boost::shared_ptr<QuantLib::RateHelper> tmp(
					new QuantLib::FixedRateBondHelper(
														QuantLib::Handle<QuantLib::Quote>(rate),
														_bond->SettlementDays,
														_bond->Nominal, 
														sch,
														coupons, 
														*bondDayCounter,
														bondConvention,
														_bond->Redemption, 
														issueDate));
				depoSwapInstruments.push_back(tmp);
			}

			if (dd0 == nullptr)
				continue;

			//
			// try to discount on 1st point, if failed - exclude the point (mark as non-enabled)
			//
			
			try
			{
				QuantLib::Date discountDate(dd0->Day, (QuantLib::Month)(dd0->Month), dd0->Year);
				// added a new entry rate. check the callibration.
				boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
					new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
						settlementDate, depoSwapInstruments,
						*termStructureDayCounter, //??? label1000
						tolerance)
				);
				QuantLib::DiscountFactor discount = 
					depoSwapTermStructure->discount(discountDate, true);	
			}
			catch (...) //the point was not good. need to remove it
			{
			//	if(depoSwapInstruments.size()>1) //if there is only 1 then we will remove everything
			//		depoSwapInstruments.pop_back();
			//	else
			//		throw gcnew Exception("depoSwapInstruments is empty. Callibration failed");
			//	ycd->entryPointList[i]->Enabled = false;

				depoSwapInstruments.pop_back();
				ycd->entryPointList[i]->Enabled = false;
				
			}

		} // for-loop

		if(depoSwapInstruments.size()==0) 
			throw gcnew Exception("callibration failed on all instruments");
	}

	void QuantLibAdaptor::CalculateDiscountedRate(YieldCurveData^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
		QuantLib::Date settlementDate(ycd->settlementDate.Day, 
										(QuantLib::Month)(ycd->settlementDate.Month), 
										ycd->settlementDate.Year);

		QuantLib::Calendar* calendar = NULL;
			
		if (!String::IsNullOrEmpty(ycd->settings->Calendar->ClassName))
		{
			if (!String::IsNullOrEmpty(ycd->settings->Calendar->MarketName)){
				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName+"::"+ycd->settings->Calendar->MarketName));}
			else
				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName));
		}
		else
			OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String("UnitedKingdom"));
		
						
		settlementDate = calendar->adjust(settlementDate);	// must be a business day
		QuantLib::Settings::instance().evaluationDate() = settlementDate;

		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
			discountDate=calendar->adjust(discountDate);	// must be a business day
			datesToDiscount[i]=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); //discountDate.dayOfMonth(),discountDate.month(), discountDate.year()
		}
		

		QuantLib::DayCounter* termStructureDayCounter = NULL;
		OBJECT_FACTORY(termStructureDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
		QuantLib::DayCounter* ZCDayCounter = NULL;
		OBJECT_FACTORY(ZCDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
		QuantLib::Compounding ZCCompounding = ConvertToCompounding(ycd->settings->ZcCompounding->ToString());

		QuantLib::DayCounter* FrwDayCounter = NULL;
		OBJECT_FACTORY(FrwDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->FrwBasisId]->ClassName)
		
		QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
	
		double tolerance = 1.0e-15;
		
		QuantLib::Frequency ZCFreq = ConvertToFrequency(ycd->settings->ZcFrequency->ToString());
		QuantLib::Frequency FrwFreq = ConvertToFrequency(ycd->settings->FrwFrequency->ToString());
		
		//
		// Calculation
		//

		// A depo-swap curve
		std::vector<boost::shared_ptr<QuantLib::RateHelper> > depoSwapInstruments;

		//this call for CalculateDiscountedRateMain with the first argument=nullptr
		//is made only to fill vector depoSwapInstruments by all entryrates
		CalculateDiscountedRateMain(nullptr, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);

		try
		{
		//	DateTime^ dd = datesToDiscount[0];
		//	QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
			QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			
			boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
				new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
						settlementDate, depoSwapInstruments,
						*termStructureDayCounter, //??? label1000
						tolerance)
				);
			QuantLib::DiscountFactor discount = 
				depoSwapTermStructure->discount(discountDate, true);
		}
		catch (...) //ok the calilbration failed lets remove problems
		{
			QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			discountDate=calendar->adjust(discountDate);
			DateTime dD=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); 
			// CalculateDiscountedRateMain(datesToDiscount[0], termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
			CalculateDiscountedRateMain(dD, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
		}

		boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
				new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
						settlementDate, depoSwapInstruments,
						*termStructureDayCounter, //??? label1000
						tolerance)
				);


		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);

			QuantLib::DiscountFactor discount = 
				depoSwapTermStructure->discount(discountDate, true);

			QuantLib::InterestRate zcRate = 
				depoSwapTermStructure->zeroRate(discountDate, *ZCDayCounter, ZCCompounding, ZCFreq, true);
			
		//	String^ termbase =(ycd->FrwTermBase);
			QuantLib::TimeUnit tu = ConvertToTimeUnit(ycd->settings->FrwTermBase->ToString());
			QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
			QuantLib::Period per(ycd->settings->FrwTerm, tu);
			QuantLib::Date forwardMaturityDate = discountDate + per;

			QuantLib::InterestRate fwdRate = 
				depoSwapTermStructure->forwardRate(discountDate, forwardMaturityDate, *FrwDayCounter, FrwCompounding, FrwFreq, true);
			
			DiscountPoint^ item = gcnew DiscountPoint();
			item->discount = discount;
			item->discountDate = *dd;
			item->zcRate = zcRate.rate();
			item->fwdRate = fwdRate.rate();

			result->Add(item);
		}
	}

	struct Datum {
        QuantLib::Date date;
        QuantLib::Rate rate;
    };

	
/*
   void QuantLibAdaptor::CalculateInflationRate(YieldCurveData^ ycd, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
		QuantLib::Date settlementDate(ycd->settlementDate.Day, 
										(QuantLib::Month)(ycd->settlementDate.Month), 
										ycd->settlementDate.Year);

		QuantLib::Calendar* calendar = NULL;
			
		if (!String::IsNullOrEmpty(ycd->settings->Calendar->ClassName))
		{
			if (!String::IsNullOrEmpty(ycd->settings->Calendar->MarketName)){
				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName+"::"+ycd->settings->Calendar->MarketName));}
			else
				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName));
		}
		else
			OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String("UnitedKingdom")); 

	//	OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String("UnitedKingdom"));
		
						
		settlementDate = calendar->adjust(settlementDate);	// must be a business day
		QuantLib::Settings::instance().evaluationDate() = settlementDate;

		
		QuantLib::Date to=settlementDate;
		QuantLib::Date from=calendar->advance(settlementDate, -32, QuantLib::Months);
		QuantLib::Schedule rpiSchedule = QuantLib::MakeSchedule().from(from).to(to).withTenor(1*QuantLib::Months).withCalendar(QuantLib::UnitedKingdom()).withConvention(QuantLib::ModifiedFollowing);

		 QuantLib::Real fixData[] = { 189.9, 189.9, 189.6, 190.5, 191.6, 192.0,
									192.2, 192.2, 192.6, 193.1, 193.3, 193.6,
									194.1, 193.4, 194.2, 195.0, 196.5, 197.7,
									198.5, 198.5, 199.2, 200.1, 200.4, 201.1,
									202.7, 201.6, 203.1, 204.4, 205.4, 206.2,
									207.3, 206.1,  -999.0 };



		QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure> hz;
				
		bool interp = false;
	//	String IndexName="UKRPI";
	//	QuantLib::InflationIndex* iiUKRPI = NULL;
	//	OBJECT_FACTORY(iiUKRPI, QuantLib::InflationIndex, IndexName);

		boost::shared_ptr<QuantLib::UKRPI> iiUKRPI(new QuantLib::UKRPI(interp, hz));
		
		for (QuantLib::Size i=0; i<rpiSchedule.size();i++) {
			iiUKRPI->addFixing(rpiSchedule[i], fixData[i]);
		}

		boost::shared_ptr<QuantLib::ZeroInflationIndex> ii = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUKRPI);
		boost::shared_ptr<QuantLib::YieldTermStructure> nominalTS = (boost::shared_ptr<QuantLib::YieldTermStructure>)(new QuantLib::FlatForward(settlementDate, 0.05, QuantLib::Actual360()));

    // now build the zero inflation curve
		Datum zcData[] = {
			{ calendar->advance(settlementDate, 1, QuantLib::Years), 2.93 },
			{ calendar->advance(settlementDate, 2, QuantLib::Years), 2.95 },
			{ calendar->advance(settlementDate, 3, QuantLib::Years), 2.965 },
			{ calendar->advance(settlementDate, 4, QuantLib::Years), 2.98 },
			{ calendar->advance(settlementDate, 5, QuantLib::Years), 3.0 },
			{ calendar->advance(settlementDate, 10, QuantLib::Years), 3.3 },
			{ calendar->advance(settlementDate, 20, QuantLib::Years), 3.5 },
			{ calendar->advance(settlementDate, 30, QuantLib::Years), 3.8 },
			{ calendar->advance(settlementDate, 50, QuantLib::Years), 4.2 }
		};

		 QuantLib::Period observationLag =  QuantLib::Period(2, QuantLib::Months);
		 QuantLib::DayCounter dc =  QuantLib::Thirty360();
		 QuantLib::Frequency frequency =  QuantLib::Monthly;
		 QuantLib::BusinessDayConvention bdc = QuantLib::ModifiedFollowing;

		 std::vector<boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > > instruments;
        for (QuantLib::Size i=0; i<LENGTH(zcData); i++) {
            QuantLib::Date maturity = zcData[i].date;
            QuantLib::Handle<QuantLib::Quote> quote(boost::shared_ptr<QuantLib::Quote>(
                new QuantLib::SimpleQuote(zcData[i].rate/100.0)));


			boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(new QuantLib::ZeroCouponInflationSwapHelper(
                quote, observationLag, maturity,
                *calendar, bdc, dc, ii));
			instruments.push_back(anInstrument);}
			



			//-----------

			
            

		QuantLib::Rate baseZeroRate = zcData[0].rate/100.0;
		boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS(
                        new QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear>(
                        settlementDate, *calendar, dc, observationLag,
                        frequency, ii->interpolated(), baseZeroRate,
                        QuantLib::Handle<QuantLib::YieldTermStructure>(nominalTS), instruments));
    pZITS->recalculate();



		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
			discountDate=calendar->adjust(discountDate);	// must be a business day
			datesToDiscount[i]=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); //discountDate.dayOfMonth(),discountDate.month(), discountDate.year()
		}



		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);

		//	QuantLib::DiscountFactor discount = 
		//		depoSwapTermStructure->discount(discountDate, true);

			QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
			bool forceLinearInterpolation = false;
			double zcRate;
			try
			{
				zcRate = (double)pZITS->zeroRate(discountDate, observationLag, forceLinearInterpolation);
			}
			catch (...)
			{
				int i=1;
			}
							
		
			double fwdRate = 100;
			QuantLib::Date bd=pZITS->baseDate();
			QuantLib::Date d = discountDate;
			QuantLib::Real z = pZITS->zeroRate(d, QuantLib::Period(0,QuantLib::Days));
			QuantLib::Real t = pZITS->dayCounter().yearFraction(bd, d);
			QuantLib::Real bf = ii->fixing(bd);

			try{
				if (t<=0)
					fwdRate = ii->fixing(d,false);
				else
				{
						t = pZITS->dayCounter().yearFraction(bd, inflationPeriod(d, ii->frequency()).first);
						fwdRate = bf * std::pow( 1+z, t);
				}
			}
			catch (...)
			{
				int dddd=1;
			}
        
			
			DiscountPoint^ item = gcnew DiscountPoint();
			item->discount = 0;
			item->discountDate = *dd;
			item->zcRate = zcRate*100;
			item->fwdRate = fwdRate;

			result->Add(item);
		}
	}
	*/


	void QuantLibAdaptor::CalculateInflationRate(InflationCurveSnapshot^ ics, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
		QuantLib::Date settlementDate(ics->settlementDate.Day, 
										(QuantLib::Month)(ics->settlementDate.Month), 
										ics->settlementDate.Year);

		QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure> hz;
				
		bool interp = false;

	//	String IndexName="UKRPI";
	//	QuantLib::InflationIndex* iiUKRPI = NULL;
	//	OBJECT_FACTORY(iiUKRPI, QuantLib::InflationIndex, IndexName);

	//	boost::shared_ptr<QuantLib::UKRPI> iiUKRPI(new QuantLib::UKRPI(interp, hz));

	//	boost::shared_ptr<QuantLib::ZeroInflationIndex> iiCPI;

		//boost::shared_ptr<QuantLib::UKRPI> iiCPI;

		boost::shared_ptr<QuantLib::ZeroInflationIndex> iiCPI;// = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiCPI);
		//iiCPI=boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(new QuantLib::UKRPI(interp, hz));

		if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("UKRPI"), StringComparison::OrdinalIgnoreCase))
		{
			boost::shared_ptr<QuantLib::UKRPI> iiUKRPI(new QuantLib::UKRPI(interp, hz));
			iiCPI=boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUKRPI);
		}
		else if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("USCPI"), StringComparison::OrdinalIgnoreCase))
		{
			boost::shared_ptr<QuantLib::USCPI> iiUSCPI(new QuantLib::USCPI(interp, hz));
			iiCPI=boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUSCPI);
		}
		else if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("GermanCPI"), StringComparison::OrdinalIgnoreCase))
		{
			bool revised=false;
			boost::shared_ptr<QuantLib::GenericCPI> iigeneric(new QuantLib::GenericCPI(QuantLib::Frequency::Monthly, revised,  
				interp,QuantLib::Period(1, QuantLib::Months),QuantLib::EURCurrency(),  hz));
			iiCPI=boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iigeneric);
		}
		else if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("ITCPI"), StringComparison::OrdinalIgnoreCase))
		{
			bool revised=false;
			boost::shared_ptr<QuantLib::GenericCPI> iigeneric(new QuantLib::GenericCPI(QuantLib::Frequency::Monthly, revised,  
				interp,QuantLib::Period(1, QuantLib::Months),QuantLib::EURCurrency(),  hz));
			iiCPI=boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iigeneric);
		}

	//	*calendar=iiCPI->fixingCalendar();
	//	QuantLib::Calendar* calendar = NULL;
		const QuantLib::Calendar& calendar = iiCPI->fixingCalendar();

//	if (!String::IsNullOrEmpty(ycd->settings->Calendar->ClassName))
//		{
//			if (!String::IsNullOrEmpty(ycd->settings->Calendar->MarketName)){
//				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName+"::"+ycd->settings->Calendar->MarketName));}
//			else
//				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(ycd->settings->Calendar->ClassName));
//		}
//		else
//			OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String("UnitedKingdom")); 

						
	//	settlementDate = calendar->adjust(settlementDate);	// must be a business day

		settlementDate = calendar.adjust(settlementDate);	// must be a business day
		QuantLib::Settings::instance().evaluationDate() = settlementDate;

	//	QuantLib::Date to=settlementDate;
	//	QuantLib::Date from=calendar->advance(settlementDate, -32, QuantLib::Months);
	//	QuantLib::Schedule rpiSchedule = QuantLib::MakeSchedule().from(from).to(to).withTenor(1*QuantLib::Months).withCalendar(QuantLib::UnitedKingdom()).withConvention(QuantLib::ModifiedFollowing);

	//	 QuantLib::Real fixData[] = { 189.9, 189.9, 189.6, 190.5, 191.6, 192.0,
	//								192.2, 192.2, 192.6, 193.1, 193.3, 193.6,
	//								194.1, 193.4, 194.2, 195.0, 196.5, 197.7,
	//								198.5, 198.5, 199.2, 200.1, 200.4, 201.1,
	//								202.7, 201.6, 203.1, 204.4, 205.4, 206.2,
	//								207.3, 206.1,  -999.0 };



		
		
	//	for (QuantLib::Size i=0; i<rpiSchedule.size();i++) {
	//		iiUKRPI->addFixing(rpiSchedule[i], fixData[i]);
	//	}

		for each(HistoricValue^ p in ics->IndexHistory)
		{
			DateTime^ d = p->Date;
			QuantLib::Date qDate(d->Day, (QuantLib::Month)(d->Month), d->Year);
			iiCPI->addFixing(qDate, p->Value);
		}

		boost::shared_ptr<QuantLib::ZeroInflationIndex> ii = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiCPI);
		boost::shared_ptr<QuantLib::YieldTermStructure> nominalTS = 
			(boost::shared_ptr<QuantLib::YieldTermStructure>)(new QuantLib::FlatForward(settlementDate, 0.05, QuantLib::Actual360()));

    // now build the zero inflation curve
		//-------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------

		 std::vector<boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > > instruments;
		 for (int i = 0; i < ics->EntryList->Count; i++)
		{
			if(ics->EntryList[i]->Type == "swap")
			{
				QuantLib::Period observationLag =  QuantLib::Period(2, QuantLib::Months);
				QuantLib::DayCounter dc =  QuantLib::Thirty360();
				QuantLib::Frequency frequency =  QuantLib::Monthly;
				QuantLib::BusinessDayConvention bdc = QuantLib::ModifiedFollowing;

			//	int term  = ics->EntryList[i]->Duration;
				InflationRate^ irate = (InflationRate^)ics->EntryList[i]->Instrument;
				int term  = irate->Duration;
				QuantLib::TimeUnit unit =ConvertToTimeUnit(irate->TimeUnit);
				QuantLib::Date maturity = calendar.advance(settlementDate, term,unit);
				
				QuantLib::Handle<QuantLib::Quote> quote(boost::shared_ptr<QuantLib::Quote>(
					new QuantLib::SimpleQuote(ics->ValueList[i]/100.0)));

				boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(
					new QuantLib::ZeroCouponInflationSwapHelper(quote, observationLag, maturity,
																calendar, bdc, dc, ii));
			
				instruments.push_back(anInstrument);
			}
		}

	//	Datum zcData[] = {
		//	{ calendar->advance(settlementDate, 1, QuantLib::Years), 2.93 },
	//		{ calendar.advance(settlementDate, 1, QuantLib::Years), 2.93 },
	//		{ calendar.advance(settlementDate, 2, QuantLib::Years), 2.95 },
	//		{ calendar.advance(settlementDate, 3, QuantLib::Years), 2.965 },
	//		{ calendar.advance(settlementDate, 4, QuantLib::Years), 2.98 },
	//		{ calendar.advance(settlementDate, 5, QuantLib::Years), 3.0 },
	//		{ calendar.advance(settlementDate, 10, QuantLib::Years), 3.3 },
	//		{ calendar.advance(settlementDate, 20, QuantLib::Years), 3.5 },
	//		{ calendar.advance(settlementDate, 30, QuantLib::Years), 3.8 },
	//		{ calendar.advance(settlementDate, 50, QuantLib::Years), 4.2 }
	//	};

     //   for (QuantLib::Size i=0; i<LENGTH(zcData); i++) {
      //      QuantLib::Date maturity = zcData[i].date;
       //     QuantLib::Handle<QuantLib::Quote> quote(boost::shared_ptr<QuantLib::Quote>(
        //        new QuantLib::SimpleQuote(zcData[i].rate/100.0)));


			//---------------
		/*	String^ type = "swap"; 
			String^ className = "swap"; 
			if (0 == String::Compare(type, gcnew String("swap"), StringComparison::OrdinalIgnoreCase))
			{
				// Rate^ _rate = dynamic_cast<Rate^>(yc->Instrument); //this is done as it might rate or bond

				if (!String::IsNullOrEmpty(className))  //ready swap rate classes from Quantlib passed by name
				{
					QuantLib::SwapIndex* swIndex = NULL;
					OBJECT_FACTORY(swIndex, QuantLib::SwapIndex, _rate->ClassName)
					boost::shared_ptr<QuantLib::SwapIndex> swapIndex(swIndex, null_deleter());
					boost::shared_ptr<QuantLib::RateHelper> tmp (
						new QuantLib::SwapRateHelper(QuantLib::Handle<QuantLib::Quote>(rate), swapIndex)
						);

					depoSwapInstruments.push_back(tmp);

					  boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(
						new QuantLib::ZeroCouponInflationSwapHelper(quote, observationLag, maturity, *calendar, bdc, dc, ii));
			
					  instruments.push_back(anInstrument);}
				}
				else //custom swap rate helpers passed by all parameters
				{
					//add here the source code for custom swap rate
				}
			}
			// epValue : type bond
			else if (0 == String::Compare(yc->Type, gcnew String("bond"), StringComparison::OrdinalIgnoreCase))
			{
				Bond^ _bond = dynamic_cast<Bond^>(yc->Instrument);
				std::vector<QuantLib::Rate> coupons(1, _bond->Coupon);
				QuantLib::DayCounter* bondDayCounter = NULL;
				OBJECT_FACTORY(bondDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[_bond->CouponBasisId]->ClassName)
				QuantLib::BusinessDayConvention bondConvention = ConvertToBusinessDayConvention(_bond->CouponConvention);
				DateTime^ issue = _bond->IssueDate;
				QuantLib::Date issueDate(issue->Day, (QuantLib::Month)(issue->Month), issue->Year);
				DateTime^ maturity = _bond->MaturityDate;
				QuantLib::Date maturityDate(maturity->Day, (QuantLib::Month)(maturity->Month), maturity->Year);
				QuantLib::Frequency bondFreq = ConvertToFrequency(_bond->CouponFrequency);

				QuantLib::Schedule sch (QuantLib::Schedule(issueDate, maturityDate,QuantLib::Period(bondFreq), 
                                        *calendar, bondConvention, bondConvention,
                                        QuantLib::DateGeneration::Backward, false)); 

				boost::shared_ptr<QuantLib::RateHelper> tmp(
					new QuantLib::FixedRateBondHelper(
														QuantLib::Handle<QuantLib::Quote>(rate),
														_bond->SettlementDays,
														_bond->Nominal, 
														sch,
														coupons, 
														*bondDayCounter,
														bondConvention,
														_bond->Redemption, 
														issueDate));
				depoSwapInstruments.push_back(tmp); */
	
//		boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(new QuantLib::ZeroCouponInflationSwapHelper(
 //               quote, observationLag, maturity,
 //               calendar, bdc, dc, ii));
			//*calendar, bdc, dc, ii));
//			instruments.push_back(anInstrument);}
			



			//-----------

		QuantLib::DayCounter dc = QuantLib::Thirty360();
		QuantLib::Frequency frequency = QuantLib::Monthly;
	//	QuantLib::Rate baseZeroRate = zcData[0].rate/100.0;
		QuantLib::Rate baseZeroRate = ics->IndexHistory[0]->Value / 100;
		QuantLib::Period observationLag = QuantLib::Period(2, QuantLib::Months);

		boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS(
                        new QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear>(
                        settlementDate, calendar, dc, observationLag,
					    frequency, ii->interpolated(), baseZeroRate,
                        QuantLib::Handle<QuantLib::YieldTermStructure>(nominalTS), instruments));

		pZITS->recalculate();

		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
			//discountDate=calendar->adjust(discountDate);	// must be a business day
			discountDate=calendar.adjust(discountDate);	// must be a business day
			datesToDiscount[i]=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); //discountDate.dayOfMonth(),discountDate.month(), discountDate.year()
		}

/*
		QuantLib::DayCounter* termStructureDayCounter = NULL;
		OBJECT_FACTORY(termStructureDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
		QuantLib::DayCounter* ZCDayCounter = NULL;
		OBJECT_FACTORY(ZCDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
		QuantLib::Compounding ZCCompounding = ConvertToCompounding(ycd->settings->ZcCompounding->ToString());

		QuantLib::DayCounter* FrwDayCounter = NULL;
		OBJECT_FACTORY(FrwDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->FrwBasisId]->ClassName)
		
		QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
	
		double tolerance = 1.0e-15;
		
		QuantLib::Frequency ZCFreq = ConvertToFrequency(ycd->settings->ZcFrequency->ToString());
		QuantLib::Frequency FrwFreq = ConvertToFrequency(ycd->settings->FrwFrequency->ToString());
		
		//
		// Calculation
		//

		// A depo-swap curve
		std::vector<boost::shared_ptr<QuantLib::RateHelper> > depoSwapInstruments;

		//this call for CalculateDiscountedRateMain with the first argument=nullptr
		//is made only to fill vector depoSwapInstruments by all entryrates
		CalculateDiscountedRateMain(nullptr, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);

		try
		{
		//	DateTime^ dd = datesToDiscount[0];
		//	QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
			QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			
			boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
				new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
						settlementDate, depoSwapInstruments,
						*termStructureDayCounter, //??? label1000
						tolerance)
				);
			QuantLib::DiscountFactor discount = 
				depoSwapTermStructure->discount(discountDate, true);
		}
		catch (...) //ok the calilbration failed lets remove problems
		{
			QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			discountDate=calendar->adjust(discountDate);
			DateTime dD=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); 
			// CalculateDiscountedRateMain(datesToDiscount[0], termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
			CalculateDiscountedRateMain(dD, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
		}

		boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
				new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
						settlementDate, depoSwapInstruments,
						*termStructureDayCounter, //??? label1000
						tolerance)
				);
		*/

		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);

		//	QuantLib::DiscountFactor discount = 
		//		depoSwapTermStructure->discount(discountDate, true);

			QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
			bool forceLinearInterpolation = false;
			double zcRate;
			try
			{
				zcRate = (double)pZITS->zeroRate(discountDate, observationLag, forceLinearInterpolation);
			}
			catch (...)
			{
				int i=1;
			}
							
		//	String^ termbase =(ycd->FrwTermBase);
		/*	QuantLib::TimeUnit tu = ConvertToTimeUnit(ycd->settings->FrwTermBase->ToString());
			QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
			QuantLib::Period per(ycd->settings->FrwTerm, tu);
			QuantLib::Date forwardMaturityDate = discountDate + per; */

			double fwdRate = 100;
			QuantLib::Date bd=pZITS->baseDate();
			QuantLib::Date d = discountDate;
			QuantLib::Real z = pZITS->zeroRate(d, QuantLib::Period(0,QuantLib::Days));
			QuantLib::Real t = pZITS->dayCounter().yearFraction(bd, d);
			QuantLib::Real bf = ii->fixing(bd);

			try
			{
				if (t <= 0)
					fwdRate = ii->fixing(d, false);
				else
				{
					t = pZITS->dayCounter().yearFraction(bd, inflationPeriod(d, ii->frequency()).first);
					fwdRate = bf * std::pow( 1 + z, t);
				}
			}
			catch (...)
			{
				int dddd = 1;
			}
        
			DiscountPoint^ item = gcnew DiscountPoint();
			item->discount = 0;
			item->discountDate = *dd;
			item->zcRate = zcRate * 100;
			item->fwdRate = fwdRate;

			result->Add(item);
		}
	}

	 void QuantLibAdaptor::CalculateInflationRate2(InflationCurveSnapshot^ ics, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
		QuantLib::Date settlementDate(ics->settlementDate.Day, 
										(QuantLib::Month)(ics->settlementDate.Month), 
										ics->settlementDate.Year);

		QuantLib::RelinkableHandle<QuantLib::ZeroInflationTermStructure> hz;
	
		bool revised=false;
		bool interp = false;
		boost::shared_ptr<QuantLib::ZeroInflationIndex> iiUKRPI;
			
		if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("UKRPI"), StringComparison::OrdinalIgnoreCase))
		{
			boost::shared_ptr<QuantLib::UKRPI> iiUKRPI1(new QuantLib::UKRPI(interp, hz));
			iiUKRPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUKRPI1);
		}
		else if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("USCPI"), StringComparison::OrdinalIgnoreCase))
		{
			boost::shared_ptr<QuantLib::USCPI> iiUKRPI1(new QuantLib::USCPI(interp, hz));
			iiUKRPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUKRPI1);
		}
		else if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("GermanCPI"), StringComparison::OrdinalIgnoreCase))
		{
			boost::shared_ptr<QuantLib::GenericCPI> iiUKRPI1(new QuantLib::GenericCPI(QuantLib::Frequency::Monthly, revised,  
				interp,QuantLib::Period(1, QuantLib::Months),QuantLib::EURCurrency(),  hz));
			iiUKRPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUKRPI1);
	 	}
		else if (0 == String::Compare(ics->InflationIndex->ClassName, gcnew String("ITCPI"), StringComparison::OrdinalIgnoreCase))
		{
			boost::shared_ptr<QuantLib::GenericCPI> iiUKRPI1(new QuantLib::GenericCPI(QuantLib::Frequency::Monthly, revised,  
				interp,QuantLib::Period(1, QuantLib::Months),QuantLib::EURCurrency(),  hz));
			iiUKRPI = boost::dynamic_pointer_cast<QuantLib::ZeroInflationIndex>(iiUKRPI1);
		} 

		const QuantLib::Calendar& calendar = iiUKRPI->fixingCalendar();

		settlementDate = calendar.adjust(settlementDate);	// must be a business day
		QuantLib::Settings::instance().evaluationDate() = settlementDate;
		
		for (int i = 0; i < ics->IndexHistory->Count; i++)
		{
			DateTime^ d=ics->IndexHistory[i]->Date;
			QuantLib::Date qDate(d->Day, (QuantLib::Month)(d->Month), d->Year);
			iiUKRPI->addFixing(qDate, ics->IndexHistory[i]->Value);
		}

		boost::shared_ptr<QuantLib::YieldTermStructure> nominalTS = 
			(boost::shared_ptr<QuantLib::YieldTermStructure>)(new QuantLib::FlatForward(settlementDate, 0.05, QuantLib::Actual360()));

    // now build the zero inflation curve
		//-------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------

		 std::vector<boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > > instruments;
		 for (int i = 0; i < ics->EntryList->Count; i++)
		{
			if(ics->EntryList[i]->Type == "swap")
			{
				QuantLib::Period observationLag = QuantLib::Period(2, QuantLib::Months);
				QuantLib::DayCounter dc = QuantLib::Thirty360();
				QuantLib::Frequency frequency =  QuantLib::Monthly;
				QuantLib::BusinessDayConvention bdc = QuantLib::ModifiedFollowing;

				InflationRate^ irate = (InflationRate^)ics->EntryList[i]->Instrument;
				int term = irate->Duration;
				QuantLib::TimeUnit unit =ConvertToTimeUnit(irate->TimeUnit);
				QuantLib::Date maturity = calendar.advance(settlementDate, term,unit);
				
				QuantLib::Handle<QuantLib::Quote> quote(boost::shared_ptr<QuantLib::Quote>(
					new QuantLib::SimpleQuote(ics->ValueList[i]/100.0)));

				boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(
					new QuantLib::ZeroCouponInflationSwapHelper(quote, observationLag, maturity,
																calendar, bdc, dc, iiUKRPI));
			
				instruments.push_back(anInstrument);
			}
		}

		QuantLib::DayCounter dc =  QuantLib::Thirty360();
		QuantLib::Frequency frequency =  QuantLib::Monthly;
		QuantLib::Rate baseZeroRate =ics->IndexHistory[0]->Value/100;
		QuantLib::Period observationLag =  QuantLib::Period(2, QuantLib::Months);
		boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS(
                        new QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear>(
                        settlementDate, calendar, dc, observationLag,
					    frequency, iiUKRPI->interpolated(), baseZeroRate,
                        QuantLib::Handle<QuantLib::YieldTermStructure>(nominalTS), instruments));
		try
		{
			pZITS->recalculate();
		}
		catch(std::exception &e)
		{
			int b = 1;
		}

		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
			discountDate = calendar.adjust(discountDate);	// must be a business day
			datesToDiscount[i] = DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); //discountDate.dayOfMonth(),discountDate.month(), discountDate.year()
		}

/*
		QuantLib::DayCounter* termStructureDayCounter = NULL;
		OBJECT_FACTORY(termStructureDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
		QuantLib::DayCounter* ZCDayCounter = NULL;
		OBJECT_FACTORY(ZCDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
		QuantLib::Compounding ZCCompounding = ConvertToCompounding(ycd->settings->ZcCompounding->ToString());

		QuantLib::DayCounter* FrwDayCounter = NULL;
		OBJECT_FACTORY(FrwDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->FrwBasisId]->ClassName)
		
		QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
	
		double tolerance = 1.0e-15;
		
		QuantLib::Frequency ZCFreq = ConvertToFrequency(ycd->settings->ZcFrequency->ToString());
		QuantLib::Frequency FrwFreq = ConvertToFrequency(ycd->settings->FrwFrequency->ToString());
		
		//
		// Calculation
		//

		// A depo-swap curve
		std::vector<boost::shared_ptr<QuantLib::RateHelper> > depoSwapInstruments;

		//this call for CalculateDiscountedRateMain with the first argument=nullptr
		//is made only to fill vector depoSwapInstruments by all entryrates
		CalculateDiscountedRateMain(nullptr, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);

		try
		{
		//	DateTime^ dd = datesToDiscount[0];
		//	QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
			QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			
			boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
				new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
						settlementDate, depoSwapInstruments,
						*termStructureDayCounter, //??? label1000
						tolerance)
				);
			QuantLib::DiscountFactor discount = 
				depoSwapTermStructure->discount(discountDate, true);
		}
		catch (...) //ok the calilbration failed lets remove problems
		{
			QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			discountDate=calendar->adjust(discountDate);
			DateTime dD=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); 
			// CalculateDiscountedRateMain(datesToDiscount[0], termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
			CalculateDiscountedRateMain(dD, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
		}

		boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
				new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
						settlementDate, depoSwapInstruments,
						*termStructureDayCounter, //??? label1000
						tolerance)
				);
		*/

		for (int i = 0; i < datesToDiscount->Count; i++)
		{
			DateTime^ dd = datesToDiscount[i];
			QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);

		//	QuantLib::DiscountFactor discount = 
		//		depoSwapTermStructure->discount(discountDate, true);

			QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
			bool forceLinearInterpolation = false;
			double zcRate;
			try
			{
				zcRate = (double)pZITS->zeroRate(discountDate, observationLag, forceLinearInterpolation);
			}
			catch (...)
			{
				int i = 1;
			}
							
		//	String^ termbase =(ycd->FrwTermBase);
		/*	QuantLib::TimeUnit tu = ConvertToTimeUnit(ycd->settings->FrwTermBase->ToString());
			QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
			QuantLib::Period per(ycd->settings->FrwTerm, tu);
			QuantLib::Date forwardMaturityDate = discountDate + per; */

			double fwdRate = 100;
			QuantLib::Date bd = pZITS->baseDate();
			QuantLib::Date d = discountDate;
			QuantLib::Real z = pZITS->zeroRate(d, QuantLib::Period(0, QuantLib::Days));
			QuantLib::Real t = pZITS->dayCounter().yearFraction(bd, d);
			QuantLib::Real bf = iiUKRPI->fixing(bd);

			try{
				if (t<=0)
					fwdRate = iiUKRPI->fixing(d,false);
				else
				{
					t = pZITS->dayCounter().yearFraction(bd, inflationPeriod(d, iiUKRPI->frequency()).first);
					fwdRate = bf * std::pow( 1 + z, t);
				}
			}
			catch (...)
			{
				int dddd = 1;
			}
        
			DiscountPoint^ item = gcnew DiscountPoint();
			item->discount = 0;
			item->discountDate = *dd;
			item->zcRate = zcRate*100;
			item->fwdRate = fwdRate;

			result->Add(item);
		}
	}

	 void QuantLibAdaptor::CalculateInflationRate1a(InflationCurveSnapshot^ ics, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
		try
		{
			QuantLib::Date settlementDate(ics->settlementDate.Day, 
											(QuantLib::Month)(ics->settlementDate.Month), 
											ics->settlementDate.Year);

			std::string className;
			MarshalString(ics->InflationIndex->ClassName, className);

			boost::shared_ptr<QuantLib::ZeroInflationIndex> iiUKRPI = 
				__QLCPP::ObjectFactory<QuantLib::ZeroInflationIndex>::CreateInflationIndexFactory(className);

			
			const QuantLib::Calendar& calendar = iiUKRPI->fixingCalendar();
			settlementDate = calendar.adjust(settlementDate);	// must be a business day
			QuantLib::Settings::instance().evaluationDate() = settlementDate;
		
			for (int i = 0; i < ics->IndexHistory->Count; i++)
			{
				DateTime^ d=ics->IndexHistory[i]->Date;
				QuantLib::Date qDate(d->Day, (QuantLib::Month)(d->Month), d->Year);
				iiUKRPI->addFixing(qDate, ics->IndexHistory[i]->Value);
			}

			boost::shared_ptr<QuantLib::YieldTermStructure> nominalTS = (boost::shared_ptr<QuantLib::YieldTermStructure>)(new QuantLib::FlatForward(settlementDate, 0.05, QuantLib::Actual360()));

		// now build the zero inflation curve
		
			std::vector<boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > > instruments;
			for (int i = 0; i < ics->EntryList->Count; i++)
			{
				if(ics->EntryList[i]->Type=="swap")
				{
					InflationRate^ irate = (InflationRate^)ics->EntryList[i]->Instrument;
					int il=irate->InflationLag;
					QuantLib::TimeUnit il_unit =ConvertToTimeUnit(irate->TimeUnit);
					QuantLib::Period observationLag =  QuantLib::Period(il, il_unit);

					QuantLib::Frequency frequency = ConvertToFrequency(irate->Frequency->ToString());
					QuantLib::DayCounter* basis = NULL;
					OBJECT_FACTORY(basis, QuantLib::DayCounter, Repository::DayCounterDic[irate->BasisId]->ClassName)
					QuantLib::BusinessDayConvention bdc = QuantLib::ModifiedFollowing;

					int term  = irate->Duration;
					QuantLib::TimeUnit unit =ConvertToTimeUnit(irate->TimeUnit);
					QuantLib::Date maturity = calendar.advance(settlementDate, term,unit);
				
					QuantLib::Handle<QuantLib::Quote> quote(boost::shared_ptr<QuantLib::Quote>(
						new QuantLib::SimpleQuote(ics->ValueList[i]/100.0)));

					boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(
						new QuantLib::ZeroCouponInflationSwapHelper(quote, observationLag, maturity,
																	calendar, bdc, *basis, iiUKRPI));
			
					instruments.push_back(anInstrument);
				}
			}

			
            
			QuantLib::DayCounter dc =  QuantLib::Thirty360();
			QuantLib::Frequency frequency =  QuantLib::Monthly;
			QuantLib::Rate baseZeroRate =ics->IndexHistory[0]->Value/100;
			QuantLib::Period observationLag =  QuantLib::Period(2, QuantLib::Months);
			boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS(
							new QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear>(
							settlementDate, calendar, dc, observationLag,
							frequency, iiUKRPI->interpolated(), baseZeroRate,
							QuantLib::Handle<QuantLib::YieldTermStructure>(nominalTS), instruments));
			
			pZITS->recalculate();

			for (int i = 0; i < datesToDiscount->Count; i++)
			{
				DateTime^ dd = datesToDiscount[i];
				QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
				discountDate=calendar.adjust(discountDate);	// must be a business day
				datesToDiscount[i]=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); //discountDate.dayOfMonth(),discountDate.month(), discountDate.year()
			}

	/*
			QuantLib::DayCounter* termStructureDayCounter = NULL;
			OBJECT_FACTORY(termStructureDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
			QuantLib::DayCounter* ZCDayCounter = NULL;
			OBJECT_FACTORY(ZCDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
			QuantLib::Compounding ZCCompounding = ConvertToCompounding(ycd->settings->ZcCompounding->ToString());

			QuantLib::DayCounter* FrwDayCounter = NULL;
			OBJECT_FACTORY(FrwDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->FrwBasisId]->ClassName)
		
			QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
	
			double tolerance = 1.0e-15;
		
			QuantLib::Frequency ZCFreq = ConvertToFrequency(ycd->settings->ZcFrequency->ToString());
			QuantLib::Frequency FrwFreq = ConvertToFrequency(ycd->settings->FrwFrequency->ToString());
		
			//
			// Calculation
			//

			// A depo-swap curve
			std::vector<boost::shared_ptr<QuantLib::RateHelper> > depoSwapInstruments;

			//this call for CalculateDiscountedRateMain with the first argument=nullptr
			//is made only to fill vector depoSwapInstruments by all entryrates
			CalculateDiscountedRateMain(nullptr, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);

			try
			{
			//	DateTime^ dd = datesToDiscount[0];
			//	QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
				QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			
				boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
					new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
							settlementDate, depoSwapInstruments,
							*termStructureDayCounter, //??? label1000
							tolerance)
					);
				QuantLib::DiscountFactor discount = 
					depoSwapTermStructure->discount(discountDate, true);
			}
			catch (...) //ok the calilbration failed lets remove problems
			{
				QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
				discountDate=calendar->adjust(discountDate);
				DateTime dD=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); 
				// CalculateDiscountedRateMain(datesToDiscount[0], termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
				CalculateDiscountedRateMain(dD, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
			}

			boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
					new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
							settlementDate, depoSwapInstruments,
							*termStructureDayCounter, //??? label1000
							tolerance)
					);
			*/

			for (int i = 0; i < datesToDiscount->Count; i++)
			{
				DateTime^ dd = datesToDiscount[i];
				QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
				QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
				bool forceLinearInterpolation = false;
				double zcRate = (double)pZITS->zeroRate(discountDate, observationLag, forceLinearInterpolation);
				
				double fwdRate = 100;
				QuantLib::Date bd=pZITS->baseDate();
				QuantLib::Date d = discountDate;
				QuantLib::Real z = pZITS->zeroRate(d, QuantLib::Period(0,QuantLib::Days));
				QuantLib::Real t = pZITS->dayCounter().yearFraction(bd, d);
				QuantLib::Real bf = iiUKRPI->fixing(bd);

				if (t<=0)
					fwdRate = iiUKRPI->fixing(d,false);
				else
				{
					t = pZITS->dayCounter().yearFraction(bd, inflationPeriod(d, iiUKRPI->frequency()).first);
					fwdRate = bf * std::pow( 1+z, t);
				}
        
			
				DiscountPoint^ item = gcnew DiscountPoint();
				item->discount = 0;
				item->discountDate = *dd;
				item->zcRate = zcRate*100;
				item->fwdRate = fwdRate;

				result->Add(item);
			}
		}
		catch (std::exception &e)
		{
			int i=1;
			std::cout << e.what() << std::endl;
		}
	}

	 void QuantLibAdaptor::CalculateInflationRate1b(InflationCurveSnapshot^ ics, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
		try
		{
			QuantLib::Date settlementDate(ics->settlementDate.Day, (QuantLib::Month)(ics->settlementDate.Month), ics->settlementDate.Year);

			std::string className;
			MarshalString(ics->InflationIndex->ClassName, className);

			boost::shared_ptr<QuantLib::ZeroInflationIndex> iiUKRPI = 
				__QLCPP::ObjectFactory<QuantLib::ZeroInflationIndex>::CreateInflationIndexFactory(className);

			
			const QuantLib::Calendar& calendar = iiUKRPI->fixingCalendar();
			settlementDate = calendar.adjust(settlementDate);	// must be a business day
			QuantLib::Settings::instance().evaluationDate() = settlementDate;
		
			for (int i = 0; i < ics->IndexHistory->Count; i++)
			{
				DateTime^ d=ics->IndexHistory[i]->Date;
				QuantLib::Date qDate(d->Day, (QuantLib::Month)(d->Month), d->Year);
				iiUKRPI->addFixing(qDate, ics->IndexHistory[i]->Value);
			}

			boost::shared_ptr<QuantLib::YieldTermStructure> nominalTS = (boost::shared_ptr<QuantLib::YieldTermStructure>)(new QuantLib::FlatForward(settlementDate, 0.05, QuantLib::Actual360()));

		// now build the zero inflation curve
		
			std::vector<boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > > instruments;
			for (int i = 0; i < ics->EntryList->Count; i++)
			{
				if(ics->EntryList[i]->Type=="swap")
				{
					InflationRate^ irate = (InflationRate^)ics->EntryList[i]->Instrument;
					int il=irate->InflationLag;
					QuantLib::TimeUnit il_unit =ConvertToTimeUnit(irate->TimeUnit);
					QuantLib::Period observationLag =  QuantLib::Period(il, il_unit);

					QuantLib::Frequency frequency = ConvertToFrequency(irate->Frequency->ToString());
					QuantLib::DayCounter* basis = NULL;
					OBJECT_FACTORY(basis, QuantLib::DayCounter, Repository::DayCounterDic[irate->BasisId]->ClassName)
					QuantLib::BusinessDayConvention bdc = QuantLib::ModifiedFollowing;

					int term  = irate->Duration;
					QuantLib::TimeUnit unit =ConvertToTimeUnit(irate->TimeUnit);
					QuantLib::Date maturity = calendar.advance(settlementDate, term,unit);
				
					QuantLib::Handle<QuantLib::Quote> quote(boost::shared_ptr<QuantLib::Quote>(
						new QuantLib::SimpleQuote(ics->ValueList[i]/100.0)));

					boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(
						new QuantLib::ZeroCouponInflationSwapHelper(quote, observationLag, maturity,
																	calendar, bdc, *basis, iiUKRPI));
			
					instruments.push_back(anInstrument);
				}
			}

			
            
			QuantLib::DayCounter dc =  QuantLib::Thirty360();
			QuantLib::Frequency frequency =  QuantLib::Monthly;
			QuantLib::Rate baseZeroRate =ics->IndexHistory[0]->Value/100;
			QuantLib::Period observationLag =  QuantLib::Period(2, QuantLib::Months);
			boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS(
							new QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear>(
							settlementDate, calendar, dc, observationLag,
							frequency, iiUKRPI->interpolated(), baseZeroRate,
							QuantLib::Handle<QuantLib::YieldTermStructure>(nominalTS), instruments));
			
			pZITS->recalculate();

			for (int i = 0; i < datesToDiscount->Count; i++)
			{
				DateTime^ dd = datesToDiscount[i];
				QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
				discountDate=calendar.adjust(discountDate);	// must be a business day
				datesToDiscount[i]=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); //discountDate.dayOfMonth(),discountDate.month(), discountDate.year()
			}

	/*
			QuantLib::DayCounter* termStructureDayCounter = NULL;
			OBJECT_FACTORY(termStructureDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
			QuantLib::DayCounter* ZCDayCounter = NULL;
			OBJECT_FACTORY(ZCDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->ZcBasisId]->ClassName)
		
			QuantLib::Compounding ZCCompounding = ConvertToCompounding(ycd->settings->ZcCompounding->ToString());

			QuantLib::DayCounter* FrwDayCounter = NULL;
			OBJECT_FACTORY(FrwDayCounter, QuantLib::DayCounter, Repository::DayCounterDic[ycd->settings->FrwBasisId]->ClassName)
		
			QuantLib::Compounding FrwCompounding = ConvertToCompounding(ycd->settings->FrwCompounding->ToString());
	
			double tolerance = 1.0e-15;
		
			QuantLib::Frequency ZCFreq = ConvertToFrequency(ycd->settings->ZcFrequency->ToString());
			QuantLib::Frequency FrwFreq = ConvertToFrequency(ycd->settings->FrwFrequency->ToString());
		
			//
			// Calculation
			//

			// A depo-swap curve
			std::vector<boost::shared_ptr<QuantLib::RateHelper> > depoSwapInstruments;

			//this call for CalculateDiscountedRateMain with the first argument=nullptr
			//is made only to fill vector depoSwapInstruments by all entryrates
			CalculateDiscountedRateMain(nullptr, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);

			try
			{
			//	DateTime^ dd = datesToDiscount[0];
			//	QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
				QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
			
				boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
					new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
							settlementDate, depoSwapInstruments,
							*termStructureDayCounter, //??? label1000
							tolerance)
					);
				QuantLib::DiscountFactor discount = 
					depoSwapTermStructure->discount(discountDate, true);
			}
			catch (...) //ok the calilbration failed lets remove problems
			{
				QuantLib::Date discountDate = calendar->advance(settlementDate, 10, QuantLib::Days);
				discountDate=calendar->adjust(discountDate);
				DateTime dD=DateTime(discountDate.year(),discountDate.month(), discountDate.dayOfMonth()); 
				// CalculateDiscountedRateMain(datesToDiscount[0], termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
				CalculateDiscountedRateMain(dD, termStructureDayCounter, tolerance, ycd, settlementDate, depoSwapInstruments);
			}

			boost::shared_ptr<QuantLib::YieldTermStructure> depoSwapTermStructure(
					new QuantLib::PiecewiseYieldCurve<QuantLib::Discount,QuantLib::LogLinear>(
							settlementDate, depoSwapInstruments,
							*termStructureDayCounter, //??? label1000
							tolerance)
					);
			*/

			for (int i = 0; i < datesToDiscount->Count; i++)
			{
				DateTime^ dd = datesToDiscount[i];
				QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
				QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
				bool forceLinearInterpolation = false;
				double zcRate = (double)pZITS->zeroRate(discountDate, observationLag, forceLinearInterpolation);
				
				double fwdRate = 100;
				QuantLib::Date bd=pZITS->baseDate();
				QuantLib::Date d = discountDate;
				QuantLib::Real z = pZITS->zeroRate(d, QuantLib::Period(0,QuantLib::Days));
				QuantLib::Real t = pZITS->dayCounter().yearFraction(bd, d);
				QuantLib::Real bf = iiUKRPI->fixing(bd);

				if (t<=0)
					fwdRate = iiUKRPI->fixing(d,false);
				else
				{
					t = pZITS->dayCounter().yearFraction(bd, inflationPeriod(d, iiUKRPI->frequency()).first);
					fwdRate = bf * std::pow( 1+z, t);
				}
        
			
				DiscountPoint^ item = gcnew DiscountPoint();
				item->discount = 0;
				item->discountDate = *dd;
				item->zcRate = zcRate*100;
				item->fwdRate = fwdRate;

				result->Add(item);
			}
		}
		catch (std::exception &e)
		{
			int i=1;
			std::cout << e.what() << std::endl;
		}
	}

	 void QuantLibAdaptor::CalculateInflationRate1(InflationCurveSnapshot^ ics, List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
		std::string className;
		MarshalString(ics->InflationIndex->ClassName, className);
		boost::shared_ptr<QuantLib::ZeroInflationIndex> InflationIndex = 
				__QLCPP::ObjectFactory<QuantLib::ZeroInflationIndex>::CreateInflationIndexFactory(className);

		try
		{
			double tolerance = 1.0e-15;
			boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS;
	//		IterativeInflationCurveCalculation(tolerance, ics, pZITS);
			pZITS->recalculate();

			for (int i = 0; i < datesToDiscount->Count; i++)
			{
				DateTime^ dd = datesToDiscount[i];
				QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
				QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
				bool forceLinearInterpolation = false;
				double zcRate = (double)pZITS->zeroRate(discountDate, observationLag, forceLinearInterpolation);
				
				double fwdRate = 100;
				QuantLib::Date bd=pZITS->baseDate();
				QuantLib::Date d = discountDate;
				QuantLib::Real z = pZITS->zeroRate(d, QuantLib::Period(0,QuantLib::Days));
				QuantLib::Real t = pZITS->dayCounter().yearFraction(bd, d);
				QuantLib::Real bf = InflationIndex->fixing(bd);

				if (t<=0)
					fwdRate = InflationIndex->fixing(d,false);
				else
				{
					t = pZITS->dayCounter().yearFraction(bd, inflationPeriod(d, InflationIndex->frequency()).first);
					fwdRate = bf * std::pow( 1+z, t);
				}
        
			
				DiscountPoint^ item = gcnew DiscountPoint();
				item->discount = 0;
				item->discountDate = *dd;
				item->zcRate = zcRate*100;
				item->fwdRate = fwdRate;

				result->Add(item);
			}
		}
		catch (std::exception &e)
		{
			int i=1;
			std::cout << e.what() << std::endl;
		}
	}

	
	
	//it will change ics and mark points breaking callibration as disabled
	void QuantLibAdaptor::IterativeInflationCurveCalculation(InflationCurveSnapshot^ ics, 
		List<DateTime>^ datesToDiscount, List<DiscountPoint^>^ result)
	{
			std::string className;
			MarshalString(ics->InflationIndex->ClassName, className);
			boost::shared_ptr<QuantLib::ZeroInflationIndex> InflationIndex = 
				__QLCPP::ObjectFactory<QuantLib::ZeroInflationIndex>::CreateInflationIndexFactory(className);
			
			const QuantLib::Calendar& calendar = InflationIndex->fixingCalendar();

			QuantLib::Date settlementDate(ics->settlementDate.Day, (QuantLib::Month)(ics->settlementDate.Month), ics->settlementDate.Year);
			settlementDate = calendar.adjust(settlementDate);	
			QuantLib::Settings::instance().evaluationDate() = settlementDate;

			for (int i = 0; i < ics->IndexHistory->Count; i++)
			{
				DateTime^ d=ics->IndexHistory[i]->Date;
				QuantLib::Date qDate(d->Day, (QuantLib::Month)(d->Month), d->Year);
				InflationIndex->addFixing(qDate, ics->IndexHistory[i]->Value);
			}

			std::vector<boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > > instruments;
			boost::shared_ptr<QuantLib::YieldTermStructure> nominalTS = (boost::shared_ptr<QuantLib::YieldTermStructure>)
				(new QuantLib::FlatForward(settlementDate, 0.05, QuantLib::Actual360()));
				
			for (int i = 0; i < ics->EntryList->Count; i++)
			{
				if(ics->EntryList[i]->Type=="swap")
				{
					InflationRate^ irate = (InflationRate^)ics->EntryList[i]->Instrument;
					int il=irate->InflationLag;
					QuantLib::TimeUnit il_unit =ConvertToTimeUnit(irate->TimeUnit);
					QuantLib::Period observationLag =  QuantLib::Period(il, il_unit);

					QuantLib::Frequency frequency = ConvertToFrequency(irate->Frequency->ToString());
					QuantLib::DayCounter* basis = NULL;
					OBJECT_FACTORY(basis, QuantLib::DayCounter, Repository::DayCounterDic[irate->BasisId]->ClassName)
					QuantLib::BusinessDayConvention bdc = QuantLib::ModifiedFollowing;

					int term  = irate->Duration;
					QuantLib::TimeUnit unit =ConvertToTimeUnit(irate->TimeUnit);
					QuantLib::Date maturity = calendar.advance(settlementDate, term,unit);

					QuantLib::Handle<QuantLib::Quote> quote(boost::shared_ptr<QuantLib::Quote>(
						new QuantLib::SimpleQuote(ics->ValueList[i]/100.0)));

					boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > anInstrument(
						new QuantLib::ZeroCouponInflationSwapHelper(quote, observationLag, maturity,
																	calendar, bdc, *basis, InflationIndex));
			
					instruments.push_back(anInstrument);
				}
			}

	
			//first we will try to callibrate using all entry rates if fails we will perform iterative process excluding rates
			QuantLib::DayCounter dc =  QuantLib::Thirty360();
			QuantLib::Frequency frequency =  QuantLib::Monthly;
			QuantLib::Rate baseZeroRate =ics->IndexHistory[0]->Value/100;
			QuantLib::Period observationLag =  QuantLib::Period(2, QuantLib::Months);
			QuantLib::Date discountDate=calendar.advance(settlementDate, 1, QuantLib::Days);
				
			boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS1(
							new QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear>(
							settlementDate, calendar, dc, observationLag,
							frequency, InflationIndex->interpolated(), baseZeroRate,
							QuantLib::Handle<QuantLib::YieldTermStructure>(nominalTS), instruments));

			try
			{
							
				pZITS1->recalculate();

				//QuantLib::DiscountFactor discount = depoSwapTermStructure->discount(discountDate, true);	
				// QuantLib::DiscountFactor discount = pZITS1->discount(discountDate, true);	
				QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
				double zcRate = (double)pZITS1->zeroRate(discountDate, observationLag, false);
				
				// means that callibration was fine with all entry rate
				// we mark all of them as enabled
				//and compute result in the end
				for (int i = 0; i < ics->EntryList->Count; i++)
				{
					ics->EntryList[i]->Enabled=true;
				}
				
			}
			catch (...) //there are rates to be excluded
			{
				std::vector<boost::shared_ptr<QuantLib::BootstrapHelper<QuantLib::ZeroInflationTermStructure> > > instrumentsIterative;

				for (int i = 0; i < instruments.size(); i++)
				{
					instrumentsIterative.push_back(instruments[i]);

					boost::shared_ptr<QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear> > pZITS1(
							new QuantLib::PiecewiseZeroInflationCurve<QuantLib::Linear>(
							settlementDate, calendar, dc, observationLag,
							frequency, InflationIndex->interpolated(), baseZeroRate,
							QuantLib::Handle<QuantLib::YieldTermStructure>(nominalTS), instrumentsIterative));

					try
					{
						pZITS1->recalculate();
						QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
						double zcRate = (double)pZITS1->zeroRate(discountDate, observationLag, false);
				
					//	QuantLib::DiscountFactor discount = depoSwapTermStructure->discount(discountDate, true);
					}
					catch (...) //there are rates to be excluded
					{
						instrumentsIterative.pop_back();
						ics->EntryList[i]->Enabled= false;
					}
				}

				if(instrumentsIterative.size()==0) 
					throw gcnew Exception("callibration failed on all instruments");
			}

			//now we have pZITS1 only with valid for callibration instruments
			//lets compute results
		
			try
			{
				for (int i = 0; i < datesToDiscount->Count; i++)
				{
					DateTime^ dd = datesToDiscount[i];
					QuantLib::Date discountDate(dd->Day, (QuantLib::Month)(dd->Month), dd->Year);
					QuantLib::Period observationLag = QuantLib::Period(2,QuantLib::Months);
					bool forceLinearInterpolation = false;
					double zcRate = (double)pZITS1->zeroRate(discountDate, observationLag, forceLinearInterpolation);
				
					double fwdRate = 100;
					QuantLib::Date bd=pZITS1->baseDate();
					QuantLib::Date d = discountDate;
					QuantLib::Real z = pZITS1->zeroRate(d, QuantLib::Period(0,QuantLib::Days));
					QuantLib::Real t = pZITS1->dayCounter().yearFraction(bd, d);
					QuantLib::Real bf = InflationIndex->fixing(bd);

					if (t<=0)
						fwdRate = InflationIndex->fixing(d,false);
					else
					{
						t = pZITS1->dayCounter().yearFraction(bd, inflationPeriod(d, InflationIndex->frequency()).first);
						fwdRate = bf * std::pow( 1+z, t);
					}
        
			
					DiscountPoint^ item = gcnew DiscountPoint();
					item->discount = 0;
					item->discountDate = *dd;
					item->zcRate = zcRate*100;
					item->fwdRate = fwdRate;

					result->Add(item);
				}
			}
			catch (std::exception &e)
			{
				int i=1;
				std::cout << e.what() << std::endl;
			}
	}

	
	bool QuantLibAdaptor::GetRateInformation(DataLayer::Rate^ rate)
	{
		QuantLib::IborIndex* dIndex = NULL;
		QuantLib::SwapIndex* swIndex = NULL;

		if (!String::IsNullOrEmpty(rate->ClassName))
		{
			if (0 == String::Compare(rate->Type, gcnew String("deposit"), StringComparison::OrdinalIgnoreCase))
				OBJECT_FACTORY(dIndex, QuantLib::IborIndex, rate->ClassName)
			else if (0 == String::Compare(rate->Type, gcnew String("swap"), StringComparison::OrdinalIgnoreCase))
				OBJECT_FACTORY(swIndex, QuantLib::SwapIndex, rate->ClassName)
		}
		
		std::ostringstream tmp;

		if (0 == String::Compare(rate->Type, gcnew String("deposit"), StringComparison::OrdinalIgnoreCase) 
			&& dIndex != NULL)
		{
			rate->Name = gcnew String(dIndex->name().c_str());
			rate->BasisId = Repository::GetDayCounterId(gcnew String(dIndex->dayCounter().name().c_str()));
			rate->FixingPlace = gcnew String(dIndex->fixingCalendar().name().c_str());
			//rate->CcyName = gcnew String(dIndex->currency().name().c_str());
			rate->SettlementDays = Int32(dIndex->fixingDays()); //gcnew Int32(dIndex->fixingDays());

			rate->Duration = Int32(dIndex->tenor().length());

			// ostringstream is used in order to create a string from enum used in Quantlib
			//all enum constructors in quantlib support output to ostream
			tmp.str("");
			tmp << QuantLib::Frequency(QuantLib::Frequency::Once) << std::ends;
			rate->Frequency = gcnew String(tmp.str().c_str());

			//	item->BusinessDayConvention=gcnew String(dIndex->businessDayConvention());
			tmp.str("");
			tmp << QuantLib::BusinessDayConvention(dIndex->businessDayConvention()) << std::ends;
			rate->BusinessDayConvention = gcnew String(tmp.str().c_str());

			tmp.str("");
			tmp << QuantLib::TimeUnit(dIndex->tenor().units()) << std::ends;
			rate->TimeUnit = gcnew String(tmp.str().c_str());

			rate->Compounding = gcnew String("Simple");  //in quantlib enum compunding doesnt support strem output

			return true;
		}
		else if(0 == String::Compare(rate->Type, gcnew String("swap"), StringComparison::OrdinalIgnoreCase) 
			&& swIndex != NULL)
		{
			rate->Name = gcnew String(swIndex->name().c_str());

			//rate->Basis = gcnew DataLayer::DayCounter(gcnew String(typeid(swIndex->dayCounter()).name()), 
			//											gcnew String(swIndex->dayCounter().name().c_str()));
			rate->BasisId = Repository::GetDayCounterId(gcnew String(swIndex->dayCounter().name().c_str()));
			rate->FixingPlace = gcnew String(swIndex->fixingCalendar().name().c_str());
			//rate->CcyName = gcnew String(swIndex->currency().name().c_str());
			rate->SettlementDays = Int32(swIndex->fixingDays());

			rate->Duration = Int32(swIndex->tenor().length());

			tmp.str("");
			tmp << QuantLib::BusinessDayConvention(swIndex->fixedLegConvention()) << std::ends;
			rate->BusinessDayConvention = gcnew String(tmp.str().c_str());

			tmp.str("");
			tmp << QuantLib::TimeUnit(swIndex->tenor().units()) << std::ends;
			rate->TimeUnit = gcnew String(tmp.str().c_str());

			rate->Compounding = gcnew String("Simple");

			rate->IndexName = gcnew String(swIndex->iborIndex()->name().c_str());

			return true;
		}
		return false;
	}

	bool QuantLibAdaptor::GetCurrencyInformation(DataLayer::Currency^ c)
	{
		QuantLib::Currency* dCurrency = NULL;

		if (!String::IsNullOrEmpty(c->ClassName))
		{
			OBJECT_FACTORY(dCurrency, QuantLib::Currency, c->ClassName)
		}

		if (dCurrency != NULL)
		{
			c->Name = gcnew String(dCurrency->name().c_str());
			c->Code = gcnew String(dCurrency->code().c_str());
			c->NumericCode = Int32(dCurrency->numericCode());
			c->FractionsPerUnit = Int32(dCurrency->fractionsPerUnit());
			c->FractionSymbol = gcnew String(dCurrency->fractionSymbol().c_str());
			c->Symbol = gcnew String(dCurrency->symbol().c_str());
			
			return true;
		}
		return false;
	}

	bool QuantLibAdaptor::GetDayCounterInformation(DataLayer::DayCounter^ d)
	{
		QuantLib::DayCounter* ptr = NULL;

		if (!String::IsNullOrEmpty(d->ClassName))
		{
			OBJECT_FACTORY(ptr, QuantLib::DayCounter, d->ClassName)
		}

		if (ptr != NULL)
		{
			d->Name = gcnew String(ptr->name().c_str());			
			return true;
		}
		return false;
	}

	std::string QuantLibAdaptor::ConvertToStdString(String^ s)
	{
		const char* chars = 
		  (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();

		std::string os = std::string(chars);
		Marshal::FreeHGlobal(IntPtr((void*)chars));

		return os;
	}

	QuantLib::TimeUnit QuantLibAdaptor::ConvertToTimeUnit(String^ s)
	{
		if(0 == String::Compare(s, "Days", StringComparison::OrdinalIgnoreCase))
			return QuantLib::TimeUnit::Days;
		else if (0 == String::Compare(s, "Weeks", StringComparison::OrdinalIgnoreCase))
			return QuantLib::TimeUnit::Weeks;
		else if (0 == String::Compare(s, "Months", StringComparison::OrdinalIgnoreCase))
			return QuantLib::TimeUnit::Months;
		else if (0 == String::Compare(s, "Years", StringComparison::OrdinalIgnoreCase))
			return QuantLib::TimeUnit::Years;
		else
			throw gcnew Exception("Not supported type of TimeUnit");;
	}

	QuantLib::Compounding QuantLibAdaptor::ConvertToCompounding(String^ s)
	{
		if(0 == String::Compare(s, "Simple", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Compounding::Simple;
		else if (0 == String::Compare(s, "Compounded", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Compounding::Compounded;
		else if (0 == String::Compare(s, "Continuous", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Compounding::Continuous;
		else if (0 == String::Compare(s, "SimpleThenCompounded", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Compounding::SimpleThenCompounded;
		else
			throw gcnew Exception("Not supported type of Compounding");;
	}

	QuantLib::BusinessDayConvention QuantLibAdaptor::ConvertToBusinessDayConvention(String^ s)
	{
		if(0 == String::Compare(s, "Following", StringComparison::OrdinalIgnoreCase))
			return QuantLib::BusinessDayConvention::Following;
		else if (0 == String::Compare(s, "ModifiedFollowing", StringComparison::OrdinalIgnoreCase))
			return QuantLib::BusinessDayConvention::ModifiedFollowing;
		else if (0 == String::Compare(s, "Preceding", StringComparison::OrdinalIgnoreCase))
			return QuantLib::BusinessDayConvention::Preceding;
		else if (0 == String::Compare(s, "ModifiedPreceding", StringComparison::OrdinalIgnoreCase))
			return QuantLib::BusinessDayConvention::ModifiedPreceding;
		else if (0 == String::Compare(s, "Unadjusted", StringComparison::OrdinalIgnoreCase))
			return QuantLib::BusinessDayConvention::Unadjusted;
		else
			throw;
	}

	QuantLib::Frequency QuantLibAdaptor::ConvertToFrequency(String^ s)
	{
		if(0 == String::Compare(s, "NoFrequency", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::NoFrequency;
		else if (0 == String::Compare(s, "Once", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Once;
		else if (0 == String::Compare(s, "Annual", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Annual;
		else if (0 == String::Compare(s, "Semiannual", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Semiannual;
		else if (0 == String::Compare(s, "EveryFourthMonth", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::EveryFourthMonth;
		else if (0 == String::Compare(s, "Quarterly", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Quarterly;
		else if (0 == String::Compare(s, "Bimonthly", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Bimonthly;
		else if (0 == String::Compare(s, "Monthly", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Monthly;
		else if (0 == String::Compare(s, "EveryFourthWeek", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::EveryFourthWeek;
		else if (0 == String::Compare(s, "Biweekly", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Biweekly;
		else if (0 == String::Compare(s, "Weekly", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Weekly;
		else if (0 == String::Compare(s, "Daily", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::Daily;
		else if (0 == String::Compare(s, "OtherFrequency", StringComparison::OrdinalIgnoreCase))
			return QuantLib::Frequency::OtherFrequency;
		else
			throw gcnew Exception("Not supported type of Frequency");;
	}

	QuantLib::Calendar QuantLibAdaptor::ConvertToCalendar(DataLayer::Calendar^ cal)
	{
			QuantLib::Calendar* calendar = NULL;
			if (!String::IsNullOrEmpty(cal->ClassName))
			{
				if (!String::IsNullOrEmpty(cal->MarketName)){
					OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(cal->ClassName+"::"+cal->MarketName));}
				else
					OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String(cal->ClassName));
			}
			else
				OBJECT_FACTORY(calendar, QuantLib::Calendar, gcnew String("UnitedKingdom"));

			return *calendar;
	}

	
	void QuantLibAdaptor::GetInstrumentMaturity(List<Instrument^>^ instruments, DateTime^ d, List<DateTime>^ datesToDiscount) //, List<DateTime^>^ result
	{
		QuantLib::Date issueDate(d->Day,	(QuantLib::Month)(d->Month), d->Year);
		
		for (int i = 0; i < instruments->Count; i++)
		{
			if(instruments[i]->Type=="InflationRate")
			{
				InflationRate^ irate = (InflationRate^)instruments[i];
			//	InflationRate^ irate = dynamic_cast<InflationRate^>(instruments[i]);
			
				int term  = irate->Duration;
				QuantLib::TimeUnit unit =ConvertToTimeUnit(irate->TimeUnit);
				const QuantLib::Calendar& calendar = ConvertToCalendar(irate->Calendar);
				QuantLib::Date maturity = calendar.advance(issueDate, term,unit);
				DateTime dt=DateTime(maturity.year(),maturity.month(), maturity.dayOfMonth()); 
				datesToDiscount->Add(dt);
			//	result
			}
		}
	}
}



    
    