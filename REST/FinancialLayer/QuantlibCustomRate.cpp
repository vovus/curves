#include "StdAfx.h"
#include "QuantlibCustomRate.h"



using namespace QuantLib;
using namespace std;




BusinessDayConvention customConvention(const Period& p) {
	switch (p.units()) {
		case Days:
        case Weeks:
			return Following;
        case Months:
        case Years:
            return ModifiedFollowing; 
        default:
            QL_FAIL("invalid time units");
	}
}

bool customEOM(const Period& p) {
	switch (p.units()) {
         case Days:
         case Weeks:
			return false;
         case Months:
         case Years:
			return true;
         default:
			QL_FAIL("invalid time units");
	}
}


 CustomIndex::CustomIndex(const std::string& familyName,
                 const Period& tenor,
                 Natural settlementDays,
                 const Currency& currency,
                 const Calendar& financialCenterCalendar,
                 const DayCounter& dayCounter,
                 const Handle<YieldTermStructure>& h)
    : IborIndex(familyName, tenor, settlementDays, currency,
                UnitedKingdom(UnitedKingdom::Exchange),  //futher development should be done here
                customConvention(tenor), customEOM(tenor),
                dayCounter, h),
      financialCenterCalendar_(financialCenterCalendar),
      jointCalendar_(JointCalendar(UnitedKingdom(UnitedKingdom::Exchange),
                                   financialCenterCalendar,
                                   JoinHolidays)) {
         
    }

Date CustomIndex::valueDate(const Date& fixingDate) const {
	QL_REQUIRE(isValidFixingDate(fixingDate),
                   "Fixing date " << fixingDate << " is not valid");

	Date d = fixingCalendar().advance(fixingDate, fixingDays_, Days);
	return jointCalendar_.adjust(d);
}

Date CustomIndex::maturityDate(const Date& valueDate) const {
	return jointCalendar_.advance(valueDate, tenor_, convention_,
                                                         endOfMonth());
 }

   
boost::shared_ptr<IborIndex> CustomIndex::clone(
                                  const Handle<YieldTermStructure>& h) const {
	return boost::shared_ptr<IborIndex>(new CustomIndex(familyName(),
                                                      tenor(),
                                                      fixingDays(),
                                                      currency(),
                                                      financialCenterCalendar_,
                                                      dayCounter(),
                                                      h));
 }