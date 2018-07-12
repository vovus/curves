#include <ql/indexes/ibor/libor.hpp>
#include <ql/indexes/ibor/audlibor.hpp> 
#include <ql/indexes/ibor/cadlibor.hpp> 
#include <ql/indexes/ibor/chflibor.hpp> 
#include <ql/indexes/ibor/dkklibor.hpp>
#include <ql/indexes/ibor/jpylibor.hpp> 
#include <ql/indexes/ibor/nzdlibor.hpp> 
#include <ql/indexes/ibor/seklibor.hpp> 
#include <ql/indexes/ibor/gbplibor.hpp>
#include <ql/indexes/ibor/usdlibor.hpp>
#include <ql/time/calendars/australia.hpp>


namespace QuantLib {
	//------------------------USD------------------------

	//! 1-week %USD %Libor index
    class USDLiborSW : public USDLibor {
      public:
        USDLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %USD %Libor index
    class USDLibor2W : public USDLibor {
      public:
        USDLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %USD %Libor index
    class USDLibor1M : public USDLibor {
      public:
        USDLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(1, Months), h) {}
    };

    //! 2-months %EUR %Libor index
    class USDLibor2M : public USDLibor {
      public:
        USDLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(2, Months), h) {}
    };

    //! 3-months %EUR %Libor index
    class USDLibor3M : public USDLibor {
      public:
        USDLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(3, Months), h) {}
    };

    //! 4-months %USD %Libor index
    class USDLibor4M : public USDLibor {
      public:
        USDLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(4, Months), h) {}
    };

    //! 5-months %USD %Libor index
    class USDLibor5M : public USDLibor {
      public:
        USDLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(5, Months), h) {}
    };

    //! 6-months %USD %Libor index
    class USDLibor6M : public USDLibor {
      public:
        USDLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(6, Months), h) {}
    };

    //! 7-months %USD %Libor index
    class USDLibor7M : public USDLibor{
      public:
        USDLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(7, Months), h) {}
    };

    //! 8-months %EUR %Libor index
    class USDLibor8M : public USDLibor {
      public:
        USDLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(8, Months), h) {}
    };

    //! 9-months %EUR %Libor index
    class USDLibor9M : public USDLibor {
      public:
        USDLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(9, Months), h) {}
    };

    //! 10-months %EUR %Libor index
    class USDLibor10M : public USDLibor {
      public:
        USDLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(10, Months), h) {}
    };

    //! 11-months %EUR %Libor index
    class USDLibor11M : public USDLibor {
      public:
        USDLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(11, Months), h) {}
    };

    //! 1-year %USD %Libor index
    class USDLibor1Y : public USDLibor {
      public:
        USDLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : USDLibor(Period(1, Years), h) {}
    };


	  //--------------------------GBP---------------------------
   //! 1-week %GBP %Libor index
    class GBPLiborSW : public GBPLibor {
      public:
        GBPLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %GBP%Libor index
    class GBPLibor2W : public GBPLibor {
      public:
        GBPLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %GBP %Libor index
    class GBPLibor1M : public GBPLibor {
      public:
        GBPLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(1, Months), h) {}
    };

    //! 2-months %EUR %Libor index
    class GBPLibor2M : public GBPLibor {
      public:
        GBPLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(2, Months), h) {}
    };

    //! 3-months %EUR %Libor index
    class GBPLibor3M : public GBPLibor {
      public:
        GBPLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(3, Months), h) {}
    };

    //! 4-months %GBP %Libor index
    class GBPLibor4M : public GBPLibor {
      public:
        GBPLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(4, Months), h) {}
    };

    //! 5-months %GBP %Libor index
    class GBPLibor5M : public GBPLibor {
      public:
        GBPLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(5, Months), h) {}
    };

    //! 6-months %GBP %Libor index
    class GBPLibor6M : public GBPLibor {
      public:
        GBPLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(6, Months), h) {}
    };

    //! 7-months %GBP %Libor index
    class GBPLibor7M : public GBPLibor{
      public:
        GBPLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(7, Months), h) {}
    };

    //! 8-months %EUR %Libor index
    class GBPLibor8M : public GBPLibor {
      public:
        GBPLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(8, Months), h) {}
    };

    //! 9-months %EUR %Libor index
    class GBPLibor9M : public GBPLibor {
      public:
        GBPLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(9, Months), h) {}
    };

    //! 10-months %EUR %Libor index
    class GBPLibor10M : public GBPLibor {
      public:
        GBPLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(10, Months), h) {}
    };

    //! 11-months %EUR %Libor index
    class GBPLibor11M : public GBPLibor {
      public:
        GBPLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(11, Months), h) {}
    };

    //! 1-year %GBP %Libor index
    class GBPLibor1Y : public GBPLibor {
      public:
        GBPLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : GBPLibor(Period(1, Years), h) {}
    };

    
// -------------  AUD deposit rates --------------

	//! base class for the one day deposit BBA %USD %LIBOR indexes
    class DailyTenorAUDLibor : public DailyTenorLibor {
      public:
        DailyTenorAUDLibor(Natural settlementDays,
                           const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorLibor("AUDLibor", settlementDays,
                          AUDCurrency(),
						  Australia(),
                          Actual360(), h) {}
    };

	 //! Overnight %USD %Libor index
    class AUDLiborON : public DailyTenorAUDLibor {
      public:
        AUDLiborON(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorAUDLibor(0, h) {}
    };


   //! 1-week %AUD %Libor index
    class AUDLiborSW : public AUDLibor {
      public:
        AUDLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %AUD %Libor index
    class AUDLibor2W : public AUDLibor {
      public:
        AUDLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %AUD %Libor index
    class AUDLibor1M : public AUDLibor {
      public:
        AUDLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(1, Months), h) {}
    };

    //! 2-months %AUD %Libor index
    class AUDLibor2M : public AUDLibor {
      public:
        AUDLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(2, Months), h) {}
    };

    //! 3-months %AUD %Libor index
    class AUDLibor3M : public AUDLibor {
      public:
        AUDLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(3, Months), h) {}
    };

    //! 4-months %AUD %Libor index
    class AUDLibor4M : public AUDLibor {
      public:
        AUDLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(4, Months), h) {}
    };

    //! 5-months %AUD %Libor index
    class AUDLibor5M : public AUDLibor {
      public:
        AUDLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(5, Months), h) {}
    };

    //! 6-months %AUD %Libor index
    class AUDLibor6M : public AUDLibor {
      public:
        AUDLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(6, Months), h) {}
    };

    //! 7-months %AUD %Libor index
    class AUDLibor7M : public AUDLibor{
      public:
        AUDLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(7, Months), h) {}
    };

    //! 8-months %AUD %Libor index
    class AUDLibor8M : public AUDLibor {
      public:
        AUDLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(8, Months), h) {}
    };

    //! 9-months %AUD %Libor index
    class AUDLibor9M : public AUDLibor {
      public:
        AUDLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(9, Months), h) {}
    };

    //! 10-months %AUD %Libor index
    class AUDLibor10M : public AUDLibor {
      public:
        AUDLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(10, Months), h) {}
    };

    //! 11-months %AUD %Libor index
    class AUDLibor11M : public AUDLibor {
      public:
        AUDLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(11, Months), h) {}
    };

    //! 1-year %AUD %Libor index
    class AUDLibor1Y : public AUDLibor {
      public:
        AUDLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : AUDLibor(Period(1, Years), h) {}
    };

  //------------------------------------------------
	

// -------------  CAD deposit rates --------------
	
	

   //! 1-week %CAD %Libor index
    class CADLiborSW : public CADLibor {
      public:
        CADLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %CAD %Libor index
    class CADLibor2W : public CADLibor {
      public:
        CADLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %CAD %Libor index
    class CADLibor1M : public CADLibor {
      public:
        CADLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(1, Months), h) {}
    };

    //! 2-months %CAD %Libor index
    class CADLibor2M : public CADLibor {
      public:
        CADLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(2, Months), h) {}
    };

    //! 3-months %CAD %Libor index
    class CADLibor3M : public CADLibor {
      public:
        CADLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(3, Months), h) {}
    };

    //! 4-months %CAD %Libor index
    class CADLibor4M : public CADLibor {
      public:
        CADLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(4, Months), h) {}
    };

    //! 5-months %CAD %Libor index
    class CADLibor5M : public CADLibor {
      public:
        CADLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(5, Months), h) {}
    };

    //! 6-months %CAD %Libor index
    class CADLibor6M : public CADLibor {
      public:
        CADLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(6, Months), h) {}
    };

    //! 7-months %CAD %Libor index
    class CADLibor7M : public CADLibor{
      public:
        CADLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(7, Months), h) {}
    };

    //! 8-months %CAD %Libor index
    class CADLibor8M : public CADLibor {
      public:
        CADLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(8, Months), h) {}
    };

    //! 9-months %CAD %Libor index
    class CADLibor9M : public CADLibor {
      public:
        CADLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(9, Months), h) {}
    };

    //! 10-months %CAD %Libor index
    class CADLibor10M : public CADLibor {
      public:
        CADLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(10, Months), h) {}
    };

    //! 11-months %CAD %Libor index
    class CADLibor11M : public CADLibor {
      public:
        CADLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(11, Months), h) {}
    };

    //! 1-year %CAD %Libor index
    class CADLibor1Y : public CADLibor {
      public:
        CADLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CADLibor(Period(1, Years), h) {}
    };

//------------------------------------------------


	//---------------------CHF---------------------

	 //! Overnight %USD %Libor index
    class CHFLiborON : public DailyTenorCHFLibor {
      public:
        CHFLiborON(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorCHFLibor(0, h) {}
    };


	//! 1-week %CHF %Libor index
    class CHFLiborSW : public CHFLibor {
      public:
        CHFLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %CHF %Libor index
    class CHFLibor2W : public CHFLibor {
      public:
        CHFLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %CHF %Libor index
    class CHFLibor1M : public CHFLibor {
      public:
        CHFLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(1, Months), h) {}
    };

    //! 2-months %CHF %Libor index
    class CHFLibor2M : public CHFLibor {
      public:
        CHFLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(2, Months), h) {}
    };

    //! 3-months %CHF %Libor index
    class CHFLibor3M : public CHFLibor {
      public:
        CHFLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(3, Months), h) {}
    };

    //! 4-months %CHF %Libor index
    class CHFLibor4M : public CHFLibor {
      public:
        CHFLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(4, Months), h) {}
    };

    //! 5-months %CHF %Libor index
    class CHFLibor5M : public CHFLibor {
      public:
        CHFLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(5, Months), h) {}
    };

    //! 6-months %CHF %Libor index
    class CHFLibor6M : public CHFLibor {
      public:
        CHFLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(6, Months), h) {}
    };

    //! 7-months %CHF %Libor index
    class CHFLibor7M : public CHFLibor{
      public:
        CHFLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(7, Months), h) {}
    };

    //! 8-months %CHF %Libor index
    class CHFLibor8M : public CHFLibor {
      public:
        CHFLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(8, Months), h) {}
    };

    //! 9-months %CHF %Libor index
    class CHFLibor9M : public CHFLibor {
      public:
        CHFLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(9, Months), h) {}
    };

    //! 10-months %CHF %Libor index
    class CHFLibor10M : public CHFLibor {
      public:
        CHFLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(10, Months), h) {}
    };

    //! 11-months %CHF %Libor index
    class CHFLibor11M : public CHFLibor {
      public:
        CHFLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(11, Months), h) {}
    };

    //! 1-year %CHF %Libor index
    class CHFLibor1Y : public CHFLibor {
      public:
        CHFLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : CHFLibor(Period(1, Years), h) {}
    };
	//------------------------------------------------

	//---------------------DKK-------------------------
	//! base class for the one day deposit BBA %USD %LIBOR indexes
    class DailyTenorDKKLibor : public DailyTenorLibor {
      public:
        DailyTenorDKKLibor(Natural settlementDays,
                           const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorLibor("DKKLibor", settlementDays,
                          DKKCurrency(),
						  Denmark(),
                          Actual360(), h) {}
    };
	
	 //! Overnight %USD %Libor index
    class DKKLiborON : public DailyTenorDKKLibor {
      public:
        DKKLiborON(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorDKKLibor(0, h) {}
    };



	//! 1-week %DKK %Libor index
    class DKKLiborSW : public DKKLibor {
      public:
        DKKLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %DKK %Libor index
    class DKKLibor2W : public DKKLibor {
      public:
        DKKLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %DKK %Libor index
    class DKKLibor1M : public DKKLibor {
      public:
        DKKLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(1, Months), h) {}
    };

    //! 2-months %DKK %Libor index
    class DKKLibor2M : public DKKLibor {
      public:
        DKKLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(2, Months), h) {}
    };

    //! 3-months %DKK %Libor index
    class DKKLibor3M : public DKKLibor {
      public:
        DKKLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(3, Months), h) {}
    };

    //! 4-months %DKK %Libor index
    class DKKLibor4M : public DKKLibor {
      public:
        DKKLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(4, Months), h) {}
    };

    //! 5-months %DKK %Libor index
    class DKKLibor5M : public DKKLibor {
      public:
        DKKLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(5, Months), h) {}
    };

    //! 6-months %DKK %Libor index
    class DKKLibor6M : public DKKLibor {
      public:
        DKKLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(6, Months), h) {}
    };

    //! 7-months %DKK %Libor index
    class DKKLibor7M : public DKKLibor{
      public:
        DKKLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(7, Months), h) {}
    };

    //! 8-months %DKK %Libor index
    class DKKLibor8M : public DKKLibor {
      public:
        DKKLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(8, Months), h) {}
    };

    //! 9-months %DKK %Libor index
    class DKKLibor9M : public DKKLibor {
      public:
        DKKLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(9, Months), h) {}
    };

    //! 10-months %DKK %Libor index
    class DKKLibor10M : public DKKLibor {
      public:
        DKKLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(10, Months), h) {}
    };

    //! 11-months %DKK %Libor index
    class DKKLibor11M : public DKKLibor {
      public:
        DKKLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(11, Months), h) {}
    };

    //! 1-year %DKK %Libor index
    class DKKLibor1Y : public DKKLibor {
      public:
        DKKLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DKKLibor(Period(1, Years), h) {}
    };

	//-------------------------JPY-----------------------
	//! Overnight %JPY %Libor index
    class JPYLiborON : public DailyTenorJPYLibor {
      public:
        JPYLiborON(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorJPYLibor(0, h) {}
    };


	
	//! 1-week %JPY %Libor index
    class JPYLiborSW : public JPYLibor {
      public:
        JPYLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %JPY %Libor index
    class JPYLibor2W : public JPYLibor {
      public:
        JPYLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %JPY %Libor index
    class JPYLibor1M : public JPYLibor {
      public:
        JPYLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(1, Months), h) {}
    };

    //! 2-months %JPY %Libor index
    class JPYLibor2M : public JPYLibor {
      public:
        JPYLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(2, Months), h) {}
    };

    //! 3-months %JPY %Libor index
    class JPYLibor3M : public JPYLibor {
      public:
        JPYLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(3, Months), h) {}
    };

    //! 4-months %JPY %Libor index
    class JPYLibor4M : public JPYLibor {
      public:
        JPYLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(4, Months), h) {}
    };

    //! 5-months %JPY %Libor index
    class JPYLibor5M : public JPYLibor {
      public:
        JPYLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(5, Months), h) {}
    };

    //! 6-months %JPY %Libor index
    class JPYLibor6M : public JPYLibor {
      public:
        JPYLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(6, Months), h) {}
    };

    //! 7-months %JPY %Libor index
    class JPYLibor7M : public JPYLibor{
      public:
        JPYLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(7, Months), h) {}
    };

    //! 8-months %JPY %Libor index
    class JPYLibor8M : public JPYLibor {
      public:
        JPYLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(8, Months), h) {}
    };

    //! 9-months %JPY %Libor index
    class JPYLibor9M : public JPYLibor {
      public:
        JPYLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(9, Months), h) {}
    };

    //! 10-months %JPY %Libor index
    class JPYLibor10M : public JPYLibor {
      public:
        JPYLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(10, Months), h) {}
    };

    //! 11-months %JPY %Libor index
    class JPYLibor11M : public JPYLibor {
      public:
        JPYLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(11, Months), h) {}
    };

    //! 1-year %JPY %Libor index
    class JPYLibor1Y : public JPYLibor {
      public:
        JPYLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JPYLibor(Period(1, Years), h) {}
    };
//----------------------------NZD-----------------
	//! base class for the one day deposit BBA %NZD %LIBOR indexes
    class DailyTenorNZDLibor : public DailyTenorLibor {
      public:
        DailyTenorNZDLibor(Natural settlementDays,
                           const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorLibor("NZDLibor", settlementDays,
                          NZDCurrency(),
						  NewZealand(),
                          Actual360(), h) {}
    };
	
	 //! Overnight %USD %Libor index
    class NZDLiborON : public DailyTenorNZDLibor {
      public:
        NZDLiborON(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorNZDLibor(0, h) {}
    };


//! 1-week %NZD %Libor index
    class NZDLiborSW : public NZDLibor {
      public:
        NZDLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %NZD %Libor index
    class NZDLibor2W : public NZDLibor {
      public:
        NZDLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %NZD %Libor index
    class NZDLibor1M : public NZDLibor {
      public:
        NZDLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(1, Months), h) {}
    };

    //! 2-months %NZD %Libor index
    class NZDLibor2M : public NZDLibor {
      public:
        NZDLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(2, Months), h) {}
    };

    //! 3-months %NZD %Libor index
    class NZDLibor3M : public NZDLibor {
      public:
        NZDLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(3, Months), h) {}
    };

    //! 4-months %NZD %Libor index
    class NZDLibor4M : public NZDLibor {
      public:
        NZDLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(4, Months), h) {}
    };

    //! 5-months %NZD %Libor index
    class NZDLibor5M : public NZDLibor {
      public:
        NZDLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(5, Months), h) {}
    };

    //! 6-months %NZD %Libor index
    class NZDLibor6M : public NZDLibor {
      public:
        NZDLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(6, Months), h) {}
    };

    //! 7-months %NZD %Libor index
    class NZDLibor7M : public NZDLibor{
      public:
        NZDLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(7, Months), h) {}
    };

    //! 8-months %NZD %Libor index
    class NZDLibor8M : public NZDLibor {
      public:
        NZDLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(8, Months), h) {}
    };

    //! 9-months %NZD %Libor index
    class NZDLibor9M : public NZDLibor {
      public:
        NZDLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(9, Months), h) {}
    };

    //! 10-months %NZD %Libor index
    class NZDLibor10M : public NZDLibor {
      public:
        NZDLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(10, Months), h) {}
    };

    //! 11-months %NZD %Libor index
    class NZDLibor11M : public NZDLibor {
      public:
        NZDLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(11, Months), h) {}
    };

    //! 1-year %NZD %Libor index
    class NZDLibor1Y : public NZDLibor {
      public:
        NZDLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : NZDLibor(Period(1, Years), h) {}
    };

//----------------------------SEK-----------------

	//! base class for the one day deposit BBA %NZD %LIBOR indexes
    class DailyTenorSEKLibor : public DailyTenorLibor {
      public:
        DailyTenorSEKLibor(Natural settlementDays,
                           const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorLibor("SEKLibor", settlementDays,
                          SEKCurrency(),
						  Sweden(),
                          Actual360(), h) {}
    };
	
	 //! Overnight %SEK %Libor index
    class SEKLiborON : public DailyTenorSEKLibor {
      public:
        SEKLiborON(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : DailyTenorSEKLibor(0, h) {}
    };
	
	
	//! 1-week %SEK %Libor index
    class SEKLiborSW : public SEKLibor {
      public:
        SEKLiborSW(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(1, Weeks), h) {}
    };

	
    //! 2-weeks %SEK %Libor index
    class SEKLibor2W : public SEKLibor {
      public:
        SEKLibor2W(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(2, Weeks), h) {}
    };


    //! 1-month %SEK %Libor index
    class SEKLibor1M : public SEKLibor {
      public:
        SEKLibor1M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(1, Months), h) {}
    };

    //! 2-months %SEK %Libor index
    class SEKLibor2M : public SEKLibor {
      public:
        SEKLibor2M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(2, Months), h) {}
    };

    //! 3-months %SEK %Libor index
    class SEKLibor3M : public SEKLibor {
      public:
        SEKLibor3M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(3, Months), h) {}
    };

    //! 4-months %SEK %Libor index
    class SEKLibor4M : public SEKLibor {
      public:
        SEKLibor4M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(4, Months), h) {}
    };

    //! 5-months %SEK %Libor index
    class SEKLibor5M : public SEKLibor {
      public:
        SEKLibor5M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(5, Months), h) {}
    };

    //! 6-months %SEK %Libor index
    class SEKLibor6M : public SEKLibor {
      public:
        SEKLibor6M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(6, Months), h) {}
    };

    //! 7-months %SEK %Libor index
    class SEKLibor7M : public SEKLibor{
      public:
        SEKLibor7M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(7, Months), h) {}
    };

    //! 8-months %SEK %Libor index
    class SEKLibor8M : public SEKLibor {
      public:
        SEKLibor8M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(8, Months), h) {}
    };

    //! 9-months %SEK %Libor index
    class SEKLibor9M : public SEKLibor {
      public:
        SEKLibor9M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(9, Months), h) {}
    };

    //! 10-months %SEK %Libor index
    class SEKLibor10M : public SEKLibor {
      public:
        SEKLibor10M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(10, Months), h) {}
    };

    //! 11-months %SEK %Libor index
    class SEKLibor11M : public SEKLibor {
      public:
        SEKLibor11M(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(11, Months), h) {}
    };

    //! 1-year %SEK %Libor index
    class SEKLibor1Y : public SEKLibor {
      public:
        SEKLibor1Y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : SEKLibor(Period(1, Years), h) {}
    };

};