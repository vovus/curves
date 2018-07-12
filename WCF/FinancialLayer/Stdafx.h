// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include <vector>
#include <utility>

#include <ql/time/calendars/all.hpp>
#include <ql/time/daycounters/all.hpp>
#include <ql/currencies/all.hpp>
#include <ql/termstructures/yield/ratehelpers.hpp>
#include <ql/termstructures/yield/piecewiseyieldcurve.hpp>
#include <ql/indexes/ibor/all.hpp>
#include <ql/indexes/ibor/usdlibor.hpp>
#include <ql/time/frequency.hpp>
#include <ql/termstructures/yield/bondhelpers.hpp>
#include <ql/pricingengines/bond/discountingbondengine.hpp>

#include <ql/termstructures/inflation/inflationhelpers.hpp>
#include <ql/termstructures/yield/flatforward.hpp>
#include <ql/termstructures/inflation/piecewisezeroinflationcurve.hpp>

#include <ql/indexes/inflation/all.hpp>
#include <ql/experimental/inflation/genericindexes.hpp>

#if (defined(WIN32)||defined(_WIN64))

#ifndef _CPPRTTI
#error( "tick RTTI option in the C++ settings of your project" )
#endif

#pragma warning (disable : 4786)
#pragma warning (disable : 4100)

#define UNIVERSAL_MAIN \
	extern "C" { __declspec( dllexport ) void EntryPoint(void *indirectionPtr);} \
	__declspec( dllexport ) \
	void EntryPoint(void *indirectionPtr)
#else
#   ifdef __cplusplus
extern "C" {
#   endif
#       define UNIVERSAL_MAIN void EntryPoint(void *indirectionPtr)
	void EntryPoint(void *indirectionPtr);
#   ifdef __cplusplus
}
#   endif
#endif
