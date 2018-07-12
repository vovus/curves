#include "StdAfx.h"

#include <ql/time/period.hpp>
#include <ql/time/timeunit.hpp>

#include "Rates.h"

using namespace __QLCPP;

//
// USD
//

USDLiborSW::USDLiborSW() 
	: QuantLib::USDLibor(QuantLib::Period(1,QuantLib::Weeks)) 
{}
USDLibor2W::USDLibor2W() 
	: QuantLib::USDLibor(QuantLib::Period(2,QuantLib::Weeks)) 
{}
USDLibor1M::USDLibor1M() 
	: QuantLib::USDLibor(QuantLib::Period(1,QuantLib::Months)) 
{}
USDLibor2M::USDLibor2M() 
	: QuantLib::USDLibor(QuantLib::Period(2,QuantLib::Months)) 
{}
USDLibor3M::USDLibor3M() 
	: QuantLib::USDLibor(QuantLib::Period(3,QuantLib::Months)) 
{}
USDLibor4M::USDLibor4M() 
	: QuantLib::USDLibor(QuantLib::Period(4,QuantLib::Months)) 
{}
USDLibor5M::USDLibor5M() 
	: QuantLib::USDLibor(QuantLib::Period(5,QuantLib::Months)) 
{}
USDLibor6M::USDLibor6M() 
	: QuantLib::USDLibor(QuantLib::Period(6,QuantLib::Months)) 
{}
USDLibor7M::USDLibor7M() 
	: QuantLib::USDLibor(QuantLib::Period(7,QuantLib::Months)) 
{}
USDLibor8M::USDLibor8M() 
	: QuantLib::USDLibor(QuantLib::Period(8,QuantLib::Months)) 
{}
USDLibor9M::USDLibor9M() 
	: QuantLib::USDLibor(QuantLib::Period(9,QuantLib::Months)) 
{}
USDLibor10M::USDLibor10M() 
	: QuantLib::USDLibor(QuantLib::Period(10,QuantLib::Months)) 
{}
USDLibor11M::USDLibor11M() 
	: QuantLib::USDLibor(QuantLib::Period(11,QuantLib::Months)) 
{}
USDLibor1Y::USDLibor1Y() 
	: QuantLib::USDLibor(QuantLib::Period(1,QuantLib::Years)) 
{}

//
// GBP
//

GBPLiborSW::GBPLiborSW() 
	: QuantLib::GBPLibor(QuantLib::Period(1,QuantLib::Weeks)) 
{}
GBPLibor2W::GBPLibor2W() 
	: QuantLib::GBPLibor(QuantLib::Period(2,QuantLib::Weeks)) 
{}
GBPLibor1M::GBPLibor1M() 
	: QuantLib::GBPLibor(QuantLib::Period(1,QuantLib::Months)) 
{}
GBPLibor2M::GBPLibor2M() 
	: QuantLib::GBPLibor(QuantLib::Period(2,QuantLib::Months)) 
{}
GBPLibor3M::GBPLibor3M() 
	: QuantLib::GBPLibor(QuantLib::Period(3,QuantLib::Months)) 
{}
GBPLibor4M::GBPLibor4M() 
	: QuantLib::GBPLibor(QuantLib::Period(4,QuantLib::Months)) 
{}
GBPLibor5M::GBPLibor5M() 
	: QuantLib::GBPLibor(QuantLib::Period(5,QuantLib::Months)) 
{}
GBPLibor6M::GBPLibor6M() 
	: QuantLib::GBPLibor(QuantLib::Period(6,QuantLib::Months)) 
{}
GBPLibor7M::GBPLibor7M() 
	: QuantLib::GBPLibor(QuantLib::Period(7,QuantLib::Months)) 
{}
GBPLibor8M::GBPLibor8M() 
	: QuantLib::GBPLibor(QuantLib::Period(8,QuantLib::Months)) 
{}
GBPLibor9M::GBPLibor9M() 
	: QuantLib::GBPLibor(QuantLib::Period(9,QuantLib::Months)) 
{}
GBPLibor10M::GBPLibor10M() 
	: QuantLib::GBPLibor(QuantLib::Period(10,QuantLib::Months)) 
{}
GBPLibor11M::GBPLibor11M() 
	: QuantLib::GBPLibor(QuantLib::Period(11,QuantLib::Months)) 
{}
GBPLibor1Y::GBPLibor1Y() 
	: QuantLib::GBPLibor(QuantLib::Period(1,QuantLib::Years)) 
{}

