#pragma once
//#ifndef quantlib_france_calendar_hpp
//#define quantlib_france_calendar_hpp

#include <ql/time/calendar.hpp>

namespace __QLCPP {

    //! France calendars
    /*! Holidays:
        <ul>
        <li>Saturdays</li>
        <li>Sundays</li>
        <li>New Year's Day, January 1st (possibly moved to Monday)</li>
        <li>Good Friday</li>
        <li>Labour Day, May 1st</li>
        <li>Christmas, December 25th</li>
       
        Other holidays for which no rule is given
        (data available for 2003-2010 only:)
        <li>Boxing Day, December 26th . NOT regular </li>
        </ul>

        Data from <http://www.euronext.com>

        */

    class France : public QuantLib::Calendar {
      private:
        class PAImpl : public QuantLib::Calendar::WesternImpl {
          public:
            std::string name() const { return "France (Paris) stock exchange"; }
            bool isBusinessDay(const QuantLib::Date&) const;
        };
      public:
        enum Market { PA    //!< Paris stock exchange
        };
        France (Market market = PA);
    };

}

// #endif