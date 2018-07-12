#include <ql/indexes/iborindex.hpp>
#include <ql/time/calendars/jointcalendar.hpp>
#include <ql/time/calendars/unitedkingdom.hpp>
#include <ql/currencies/europe.hpp>

namespace QuantLib {

  
    class CustomIndex : public IborIndex {
      public:
        CustomIndex(const std::string& familyName,
              const Period& tenor,
              Natural settlementDays,
              const Currency& currency,
              const Calendar& financialCenterCalendar,
              const DayCounter& dayCounter,
              const Handle<YieldTermStructure>& h =
                                    Handle<YieldTermStructure>());
       
        Date valueDate(const Date& fixingDate) const;
        Date maturityDate(const Date& valueDate) const;
        boost::shared_ptr<IborIndex> clone(
                                   const Handle<YieldTermStructure>& h) const;
        // @}
      private:
        Calendar financialCenterCalendar_;
        Calendar jointCalendar_;
    };

}