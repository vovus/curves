#include "StdAfx.h"
#include "Calendars.h"
#include <ql/errors.hpp>
using namespace __QLCPP;

namespace __QLCPP { 
	
    France::France(France::Market market) {
        // all calendar instances share the same implementation instance
        static boost::shared_ptr<Calendar::Impl> paImpl(new France::PAImpl);
        switch (market) {
          case PA:
            impl_ = paImpl;
            break;
          default:
            QL_FAIL("unknown market");
        }
    }

    bool France::PAImpl::isBusinessDay(const QuantLib::Date& date) const {
        QuantLib::Weekday w = date.weekday();
        QuantLib::Day d = date.dayOfMonth(), dd = date.dayOfYear();
        QuantLib::Month m = date.month();
        QuantLib::Year y = date.year();
        QuantLib::Day em = easterMonday(y);

        if (isWeekend(w)
            // New Year's Day
            || (d == 1 && m == QuantLib::January)
             // Good Friday
            || (dd == em-3)
            // Easter Monday
            || (dd == em)
            // Labor Day
            || (d == 1 && m == QuantLib::May)
            // National Day
            || (d == 14 && w == QuantLib::July))
            return false;

        if ((y == 2003)
		 || (y == 2005)
		 || (y == 2006)
		 || (y == 2007)
		 || (y == 2008)
		 || (y == 2009)){
	     if (// Boxing Day
               (d==26 && m == QuantLib::December)
		 )
	      return false;
        }

       return true;
    }
}
