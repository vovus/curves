#include <ql/indexes/swap/eurliborswap.hpp>
#include <ql/indexes/swap/euriborswap.hpp> 
#include <ql/time/calendars/unitedkingdom.hpp>
#include <ql/time/daycounters/actual365fixed.hpp>
#include <ql/currencies/europe.hpp>
#include <ql/indexes/ibor/gbplibor.hpp>
#include <ql/indexes/swap/gbpliborswap.hpp> 
#include <ql/indexes/swap/usdliborswap.hpp>
#include <ql/indexes/swap/jpyliborswap.hpp> 
#include <ql/indexes/swap/chfliborswap.hpp> 






namespace QuantLib {


	//--------------------- EurLiborSwapIsdaFixA ---------------------------
    
	class EurLiborSwapIsdaFixA1y : public EurLiborSwapIsdaFixA {
      public:
        EurLiborSwapIsdaFixA1y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixA(Period(1, Years), h) {}
    };

    class EurLiborSwapIsdaFixA2y : public EurLiborSwapIsdaFixA {
      public:
        EurLiborSwapIsdaFixA2y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixA(Period(2, Years), h) {}
    };

	class EurLiborSwapIsdaFixA3y : public EurLiborSwapIsdaFixA {
      public:
        EurLiborSwapIsdaFixA3y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixA(Period(3, Years), h) {}
    };

	class EurLiborSwapIsdaFixA4y : public EurLiborSwapIsdaFixA {
      public:
        EurLiborSwapIsdaFixA4y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixA(Period(4, Years), h) {}
    };

	class EurLiborSwapIsdaFixA5y : public EurLiborSwapIsdaFixA {
      public:
        EurLiborSwapIsdaFixA5y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixA(Period(5, Years), h) {}
    };
	class EurLiborSwapIsdaFixA6y : public EurLiborSwapIsdaFixA {
      public:
        EurLiborSwapIsdaFixA6y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixA(Period(6, Years), h) {}
    };
	class EurLiborSwapIsdaFixA7y : public EurLiborSwapIsdaFixA {
      public:
        EurLiborSwapIsdaFixA7y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixA(Period(7, Years), h) {}
	};
	  class EurLiborSwapIsdaFixA8y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(8, Years), h) {}
		};
	  class EurLiborSwapIsdaFixA9y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(9, Years), h) {}
		};
	 class EurLiborSwapIsdaFixA10y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(10, Years), h) {}
		};
	class EurLiborSwapIsdaFixA12y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(12, Years), h) {}
		};
	class EurLiborSwapIsdaFixA15y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(15, Years), h) {}
		};
	class EurLiborSwapIsdaFixA20y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(20, Years), h) {}
		};
	class EurLiborSwapIsdaFixA25y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(25, Years), h) {}
		};
	class EurLiborSwapIsdaFixA30y : public EurLiborSwapIsdaFixA {
		  public:
			EurLiborSwapIsdaFixA30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixA(Period(30, Years), h) {}
		};

	//--------------------- EurLiborSwapIsdaFixB ---------------------------

	class EurLiborSwapIsdaFixB1y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB1y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(1, Years), h) {}
		};

    class EurLiborSwapIsdaFixB2y : public EurLiborSwapIsdaFixB {
      public:
        EurLiborSwapIsdaFixB2y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixB(Period(2, Years), h) {}
    };

	class EurLiborSwapIsdaFixB3y : public EurLiborSwapIsdaFixB {
      public:
        EurLiborSwapIsdaFixB3y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixB(Period(3, Years), h) {}
    };

	class EurLiborSwapIsdaFixB4y : public EurLiborSwapIsdaFixB {
      public:
        EurLiborSwapIsdaFixB4y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixB(Period(4, Years), h) {}
    };

	class EurLiborSwapIsdaFixB5y : public EurLiborSwapIsdaFixB {
      public:
        EurLiborSwapIsdaFixB5y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixB(Period(5, Years), h) {}
    };
	class EurLiborSwapIsdaFixB6y : public EurLiborSwapIsdaFixB {
      public:
        EurLiborSwapIsdaFixB6y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixB(Period(6, Years), h) {}
    };
	class EurLiborSwapIsdaFixB7y : public EurLiborSwapIsdaFixB {
      public:
        EurLiborSwapIsdaFixB7y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : EurLiborSwapIsdaFixB(Period(7, Years), h) {}
    };
	  class EurLiborSwapIsdaFixB8y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(8, Years), h) {}
		};
	  class EurLiborSwapIsdaFixB9y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(9, Years), h) {}
		};
	 class EurLiborSwapIsdaFixB10y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(10, Years), h) {}
		};
	class EurLiborSwapIsdaFixB12y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(12, Years), h) {}
		};
	class EurLiborSwapIsdaFixB15y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(15, Years), h) {}
		};
	class EurLiborSwapIsdaFixB20y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(20, Years), h) {}
		};
	class EurLiborSwapIsdaFixB25y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(25, Years), h) {}
		};
	class EurLiborSwapIsdaFixB30y : public EurLiborSwapIsdaFixB {
		  public:
			EurLiborSwapIsdaFixB30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIsdaFixB(Period(30, Years), h) {}
		};

	// -------------------------  EurLiborSwapIfrFix  -----------------------

		class EurLiborSwapIfrFix1y : public EurLiborSwapIfrFix {
			  public:
				EurLiborSwapIfrFix1y(const Handle<YieldTermStructure>& h =
											Handle<YieldTermStructure>())
				: EurLiborSwapIfrFix(Period(1, Years), h) {}
			};

		class EurLiborSwapIfrFix2y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix2y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(2, Years), h) {}
		};

		class EurLiborSwapIfrFix3y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix3y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(3, Years), h) {}
		};

		class EurLiborSwapIfrFix4y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix4y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(4, Years), h) {}
		};

		class EurLiborSwapIfrFix5y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix5y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(5, Years), h) {}
		};
		class EurLiborSwapIfrFix6y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix6y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(6, Years), h) {}
		};
		class EurLiborSwapIfrFix7y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix7y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(7, Years), h) {}
		};
	  class EurLiborSwapIfrFix8y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(8, Years), h) {}
		};
	  class EurLiborSwapIfrFix9y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(9, Years), h) {}
		};
	 class EurLiborSwapIfrFix10y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(10, Years), h) {}
		};
	class EurLiborSwapIfrFix12y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(12, Years), h) {}
		};
	class EurLiborSwapIfrFix15y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(15, Years), h) {}
		};
	class EurLiborSwapIfrFix20y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(20, Years), h) {}
		};
	class EurLiborSwapIfrFix25y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(25, Years), h) {}
		};
	class EurLiborSwapIfrFix30y : public EurLiborSwapIfrFix {
		  public:
			EurLiborSwapIfrFix30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EurLiborSwapIfrFix(Period(30, Years), h) {}
		};

	//--------------------- EuriborSwapIsdaFixA ---------------------------

	class EuriborSwapIsdaFixA1y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA1y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(1, Years), h) {}
		};

		class EuriborSwapIsdaFixA2y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA2y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(2, Years), h) {}
		};

		class EuriborSwapIsdaFixA3y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA3y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(3, Years), h) {}
		};

		class EuriborSwapIsdaFixA4y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA4y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(4, Years), h) {}
		};

		class EuriborSwapIsdaFixA5y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA5y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(5, Years), h) {}
		};
		class EuriborSwapIsdaFixA6y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA6y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(6, Years), h) {}
		};
		class EuriborSwapIsdaFixA7y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA7y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(7, Years), h) {}
		};
	  class EuriborSwapIsdaFixA8y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(8, Years), h) {}
		};
	  class EuriborSwapIsdaFixA9y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(9, Years), h) {}
		};
	 class EuriborSwapIsdaFixA10y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(10, Years), h) {}
		};
	class EuriborSwapIsdaFixA12y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(12, Years), h) {}
		};
	class EuriborSwapIsdaFixA15y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(15, Years), h) {}
		};
	class EuriborSwapIsdaFixA20y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(20, Years), h) {}
		};
	class EuriborSwapIsdaFixA25y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(25, Years), h) {}
		};
	class EuriborSwapIsdaFixA30y : public EuriborSwapIsdaFixA {
		  public:
			EuriborSwapIsdaFixA30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixA(Period(30, Years), h) {}
		};


		//------------------------------EuriborSwapIsdaFixB -------------------------
	class EuriborSwapIsdaFixB1y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB1y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(1, Years), h) {}
		};

		class EuriborSwapIsdaFixB2y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB2y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(2, Years), h) {}
		};

		class EuriborSwapIsdaFixB3y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB3y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(3, Years), h) {}
		};

		class EuriborSwapIsdaFixB4y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB4y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(4, Years), h) {}
		};

		class EuriborSwapIsdaFixB5y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB5y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(5, Years), h) {}
		};
		class EuriborSwapIsdaFixB6y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB6y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(6, Years), h) {}
		};
		class EuriborSwapIsdaFixB7y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB7y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(7, Years), h) {}
		};
	  class EuriborSwapIsdaFixB8y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(8, Years), h) {}
		};
	  class EuriborSwapIsdaFixB9y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(9, Years), h) {}
		};
	 class EuriborSwapIsdaFixB10y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(10, Years), h) {}
		};
	class EuriborSwapIsdaFixB12y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(12, Years), h) {}
		};
	class EuriborSwapIsdaFixB15y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(15, Years), h) {}
		};
	class EuriborSwapIsdaFixB20y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(20, Years), h) {}
		};
	class EuriborSwapIsdaFixB25y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(25, Years), h) {}
			
		};
	class EuriborSwapIsdaFixB30y : public EuriborSwapIsdaFixB {
		  public:
			EuriborSwapIsdaFixB30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: EuriborSwapIsdaFixB(Period(30, Years), h) {}
		};

//-------------------------GBP swap rates------------
		class GbpLiborSwapIsdaFix1y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix1y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(1, Years), h) {}
		};

		class GbpLiborSwapIsdaFix2y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix2y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(2, Years), h) {}
		};

		class GbpLiborSwapIsdaFix3y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix3y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(3, Years), h) {}
		};

		class GbpLiborSwapIsdaFix4y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix4y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(4, Years), h) {}
		};

		class GbpLiborSwapIsdaFix5y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix5y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(5, Years), h) {}
		};
		class GbpLiborSwapIsdaFix6y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix6y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(6, Years), h) {}
		};
		class GbpLiborSwapIsdaFix7y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix7y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(7, Years), h) {}
		};
	  class GbpLiborSwapIsdaFix8y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(8, Years), h) {}
		};
	  class GbpLiborSwapIsdaFix9y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(9, Years), h) {}
		};
	 class GbpLiborSwapIsdaFix10y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(10, Years), h) {}
		};
	class GbpLiborSwapIsdaFix12y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(12, Years), h) {}
		};
	class GbpLiborSwapIsdaFix15y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(15, Years), h) {}
		};
	class GbpLiborSwapIsdaFix20y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(20, Years), h) {}
		};
	class GbpLiborSwapIsdaFix25y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(25, Years), h) {}
		};
	class GbpLiborSwapIsdaFix30y : public GbpLiborSwapIsdaFix {
		  public:
			GbpLiborSwapIsdaFix30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: GbpLiborSwapIsdaFix(Period(30, Years), h) {}
		};

//---------------------------usd swap rates------------
	class UsdLiborSwapIsdaFixAm1y : public UsdLiborSwapIsdaFixAm {
      public:
        UsdLiborSwapIsdaFixAm1y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixAm(Period(1, Years), h) {}
    };

    class UsdLiborSwapIsdaFixAm2y : public UsdLiborSwapIsdaFixAm {
      public:
        UsdLiborSwapIsdaFixAm2y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixAm(Period(2, Years), h) {}
    };

	class UsdLiborSwapIsdaFixAm3y : public UsdLiborSwapIsdaFixAm {
      public:
        UsdLiborSwapIsdaFixAm3y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixAm(Period(3, Years), h) {}
    };

	class UsdLiborSwapIsdaFixAm4y : public UsdLiborSwapIsdaFixAm {
      public:
        UsdLiborSwapIsdaFixAm4y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixAm(Period(4, Years), h) {}
    };

	class UsdLiborSwapIsdaFixAm5y : public UsdLiborSwapIsdaFixAm {
      public:
        UsdLiborSwapIsdaFixAm5y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixAm(Period(5, Years), h) {}
    };
	class UsdLiborSwapIsdaFixAm6y : public UsdLiborSwapIsdaFixAm {
      public:
        UsdLiborSwapIsdaFixAm6y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixAm(Period(6, Years), h) {}
    };
	class UsdLiborSwapIsdaFixAm7y : public UsdLiborSwapIsdaFixAm {
      public:
        UsdLiborSwapIsdaFixAm7y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixAm(Period(7, Years), h) {}
	};
	  class UsdLiborSwapIsdaFixAm8y : public UsdLiborSwapIsdaFixAm {
		  public:
			UsdLiborSwapIsdaFixAm8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixAm(Period(8, Years), h) {}
		};
	  class UsdLiborSwapIsdaFixAm9y : public UsdLiborSwapIsdaFixAm {
		  public:
			UsdLiborSwapIsdaFixAm9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixAm(Period(9, Years), h) {}
		};
	 class UsdLiborSwapIsdaFixAm10y : public UsdLiborSwapIsdaFixAm {
		  public:
			UsdLiborSwapIsdaFixAm10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixAm(Period(10, Years), h) {}
		};
	
	class UsdLiborSwapIsdaFixAm15y : public UsdLiborSwapIsdaFixAm {
		  public:
			UsdLiborSwapIsdaFixAm15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixAm(Period(15, Years), h) {}
		};
	class UsdLiborSwapIsdaFixAm20y : public UsdLiborSwapIsdaFixAm {
		  public:
			UsdLiborSwapIsdaFixAm20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixAm(Period(20, Years), h) {}
		};
	
	class UsdLiborSwapIsdaFixAm30y : public UsdLiborSwapIsdaFixAm {
		  public:
			UsdLiborSwapIsdaFixAm30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixAm(Period(30, Years), h) {}
		};

	//---------------
	class UsdLiborSwapIsdaFixPm1y : public UsdLiborSwapIsdaFixPm {
      public:
        UsdLiborSwapIsdaFixPm1y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixPm(Period(1, Years), h) {}
    };

    class UsdLiborSwapIsdaFixPm2y : public UsdLiborSwapIsdaFixPm {
      public:
        UsdLiborSwapIsdaFixPm2y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixPm(Period(2, Years), h) {}
    };

	class UsdLiborSwapIsdaFixPm3y : public UsdLiborSwapIsdaFixPm {
      public:
        UsdLiborSwapIsdaFixPm3y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixPm(Period(3, Years), h) {}
    };

	class UsdLiborSwapIsdaFixPm4y : public UsdLiborSwapIsdaFixPm {
      public:
        UsdLiborSwapIsdaFixPm4y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixPm(Period(4, Years), h) {}
    };

	class UsdLiborSwapIsdaFixPm5y : public UsdLiborSwapIsdaFixPm {
      public:
        UsdLiborSwapIsdaFixPm5y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixPm(Period(5, Years), h) {}
    };
	class UsdLiborSwapIsdaFixPm6y : public UsdLiborSwapIsdaFixPm {
      public:
        UsdLiborSwapIsdaFixPm6y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixPm(Period(6, Years), h) {}
    };
	class UsdLiborSwapIsdaFixPm7y : public UsdLiborSwapIsdaFixPm {
      public:
        UsdLiborSwapIsdaFixPm7y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : UsdLiborSwapIsdaFixPm(Period(7, Years), h) {}
	};
	  class UsdLiborSwapIsdaFixPm8y : public UsdLiborSwapIsdaFixPm {
		  public:
			UsdLiborSwapIsdaFixPm8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixPm(Period(8, Years), h) {}
		};
	  class UsdLiborSwapIsdaFixPm9y : public UsdLiborSwapIsdaFixPm {
		  public:
			UsdLiborSwapIsdaFixPm9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixPm(Period(9, Years), h) {}
		};
	 class UsdLiborSwapIsdaFixPm10y : public UsdLiborSwapIsdaFixPm {
		  public:
			UsdLiborSwapIsdaFixPm10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixPm(Period(10, Years), h) {}
		};
	
	class UsdLiborSwapIsdaFixPm15y : public UsdLiborSwapIsdaFixPm {
		  public:
			UsdLiborSwapIsdaFixPm15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixPm(Period(15, Years), h) {}
		};
	class UsdLiborSwapIsdaFixPm20y : public UsdLiborSwapIsdaFixPm {
		  public:
			UsdLiborSwapIsdaFixPm20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixPm(Period(20, Years), h) {}
		};
		
	class UsdLiborSwapIsdaFixPm30y : public UsdLiborSwapIsdaFixPm {
		  public:
			UsdLiborSwapIsdaFixPm30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: UsdLiborSwapIsdaFixPm(Period(30, Years), h) {}
		};

	//-------------  JPY swap rates -
		class JpyLiborSwapIsdaFixAm1y : public JpyLiborSwapIsdaFixAm {
      public:
        JpyLiborSwapIsdaFixAm1y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixAm(Period(1, Years), h) {}
    };

    class JpyLiborSwapIsdaFixAm2y : public JpyLiborSwapIsdaFixAm {
      public:
        JpyLiborSwapIsdaFixAm2y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixAm(Period(2, Years), h) {}
    };

	class JpyLiborSwapIsdaFixAm3y : public JpyLiborSwapIsdaFixAm {
      public:
        JpyLiborSwapIsdaFixAm3y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixAm(Period(3, Years), h) {}
    };

	class JpyLiborSwapIsdaFixAm4y : public JpyLiborSwapIsdaFixAm {
      public:
        JpyLiborSwapIsdaFixAm4y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixAm(Period(4, Years), h) {}
    };

	class JpyLiborSwapIsdaFixAm5y : public JpyLiborSwapIsdaFixAm {
      public:
        JpyLiborSwapIsdaFixAm5y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixAm(Period(5, Years), h) {}
    };
	class JpyLiborSwapIsdaFixAm6y : public JpyLiborSwapIsdaFixAm {
      public:
        JpyLiborSwapIsdaFixAm6y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixAm(Period(6, Years), h) {}
    };
	class JpyLiborSwapIsdaFixAm7y : public JpyLiborSwapIsdaFixAm {
      public:
        JpyLiborSwapIsdaFixAm7y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixAm(Period(7, Years), h) {}
	};
	  class JpyLiborSwapIsdaFixAm8y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(8, Years), h) {}
		};
	  class JpyLiborSwapIsdaFixAm9y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(9, Years), h) {}
		};
	 class JpyLiborSwapIsdaFixAm10y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(10, Years), h) {}
		};
	class JpyLiborSwapIsdaFixAm12y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(12, Years), h) {}
		};
	class JpyLiborSwapIsdaFixAm15y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(15, Years), h) {}
		};
	class JpyLiborSwapIsdaFixAm20y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(20, Years), h) {}
		};
	class JpyLiborSwapIsdaFixAm25y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(25, Years), h) {}
		};
	class JpyLiborSwapIsdaFixAm30y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(30, Years), h) {}
		};
	class JpyLiborSwapIsdaFixAm35y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm35y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(35, Years), h) {}
		};
	class JpyLiborSwapIsdaFixAm40y : public JpyLiborSwapIsdaFixAm {
		  public:
			JpyLiborSwapIsdaFixAm40y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixAm(Period(40, Years), h) {}
		};



//----------
		class JpyLiborSwapIsdaFixPm1y : public JpyLiborSwapIsdaFixPm {
      public:
        JpyLiborSwapIsdaFixPm1y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixPm(Period(1, Years), h) {}
    };

    class JpyLiborSwapIsdaFixPm2y : public JpyLiborSwapIsdaFixPm {
      public:
        JpyLiborSwapIsdaFixPm2y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixPm(Period(2, Years), h) {}
    };

	class JpyLiborSwapIsdaFixPm3y : public JpyLiborSwapIsdaFixPm {
      public:
        JpyLiborSwapIsdaFixPm3y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixPm(Period(3, Years), h) {}
    };

	class JpyLiborSwapIsdaFixPm4y : public JpyLiborSwapIsdaFixPm {
      public:
        JpyLiborSwapIsdaFixPm4y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixPm(Period(4, Years), h) {}
    };

	class JpyLiborSwapIsdaFixPm5y : public JpyLiborSwapIsdaFixPm {
      public:
        JpyLiborSwapIsdaFixPm5y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixPm(Period(5, Years), h) {}
    };
	class JpyLiborSwapIsdaFixPm6y : public JpyLiborSwapIsdaFixPm {
      public:
        JpyLiborSwapIsdaFixPm6y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixPm(Period(6, Years), h) {}
    };
	class JpyLiborSwapIsdaFixPm7y : public JpyLiborSwapIsdaFixPm {
      public:
        JpyLiborSwapIsdaFixPm7y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : JpyLiborSwapIsdaFixPm(Period(7, Years), h) {}
	};
	  class JpyLiborSwapIsdaFixPm8y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(8, Years), h) {}
		};
	  class JpyLiborSwapIsdaFixPm9y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(9, Years), h) {}
		};
	 class JpyLiborSwapIsdaFixPm10y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(10, Years), h) {}
		};
	class JpyLiborSwapIsdaFixPm12y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm12y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(12, Years), h) {}
		};
	class JpyLiborSwapIsdaFixPm15y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm15y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(15, Years), h) {}
		};
	class JpyLiborSwapIsdaFixPm20y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm20y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(20, Years), h) {}
		};
	class JpyLiborSwapIsdaFixPm25y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm25y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(25, Years), h) {}
		};
	class JpyLiborSwapIsdaFixPm30y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm30y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(30, Years), h) {}
		};
	class JpyLiborSwapIsdaFixPm35y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm35y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(35, Years), h) {}
		};
	class JpyLiborSwapIsdaFixPm40y : public JpyLiborSwapIsdaFixPm {
		  public:
			JpyLiborSwapIsdaFixPm40y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: JpyLiborSwapIsdaFixPm(Period(40, Years), h) {}
		};
//---------------------  chf

	class ChfLiborSwapIsdaFix1y : public ChfLiborSwapIsdaFix {
      public:
        ChfLiborSwapIsdaFix1y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : ChfLiborSwapIsdaFix(Period(1, Years), h) {}
    };

    class ChfLiborSwapIsdaFix2y : public ChfLiborSwapIsdaFix {
      public:
        ChfLiborSwapIsdaFix2y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : ChfLiborSwapIsdaFix(Period(2, Years), h) {}
    };

	class ChfLiborSwapIsdaFix3y : public ChfLiborSwapIsdaFix {
      public:
        ChfLiborSwapIsdaFix3y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : ChfLiborSwapIsdaFix(Period(3, Years), h) {}
    };

	class ChfLiborSwapIsdaFix4y : public ChfLiborSwapIsdaFix {
      public:
        ChfLiborSwapIsdaFix4y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : ChfLiborSwapIsdaFix(Period(4, Years), h) {}
    };

	class ChfLiborSwapIsdaFix5y : public ChfLiborSwapIsdaFix {
      public:
        ChfLiborSwapIsdaFix5y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : ChfLiborSwapIsdaFix(Period(5, Years), h) {}
    };
	class ChfLiborSwapIsdaFix6y : public ChfLiborSwapIsdaFix {
      public:
        ChfLiborSwapIsdaFix6y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : ChfLiborSwapIsdaFix(Period(6, Years), h) {}
    };
	class ChfLiborSwapIsdaFix7y : public ChfLiborSwapIsdaFix {
      public:
        ChfLiborSwapIsdaFix7y(const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>())
        : ChfLiborSwapIsdaFix(Period(7, Years), h) {}
	};
	  class ChfLiborSwapIsdaFix8y : public ChfLiborSwapIsdaFix {
		  public:
			ChfLiborSwapIsdaFix8y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: ChfLiborSwapIsdaFix(Period(8, Years), h) {}
		};
	  class ChfLiborSwapIsdaFix9y : public ChfLiborSwapIsdaFix {
		  public:
			ChfLiborSwapIsdaFix9y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: ChfLiborSwapIsdaFix(Period(9, Years), h) {}
		};
	 class ChfLiborSwapIsdaFix10y : public ChfLiborSwapIsdaFix {
		  public:
			ChfLiborSwapIsdaFix10y(const Handle<YieldTermStructure>& h =
										Handle<YieldTermStructure>())
			: ChfLiborSwapIsdaFix(Period(10, Years), h) {}
		};



};