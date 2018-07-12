#pragma once

//
// Example:
//
// QuantLib::Calendar* cal = OBJECT_FACTORY(cal, QuantLib::Calendar, QuantLib::UnitedKingdom::Settlement)
//
// NOTE: no commas at the end !!!
// 
// OR:
//
// QuantLib::Calendar* cal = OBJECT_FACTORY(cal, QuantLib::Calendar, QuantLib::Argentina)
//
// NOTE: in the 1st example we reffer to the callendar created with parameter, in 2nd case - to default constructor (no parameter)
//

#include <boost/any.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/algorithm/string.hpp>
#include <map>

namespace __QLCPP 
{
	class ObjectFactory2
	{
		static std::map<std::string, void* > sFactoryMap;
		
		template <class T, class P>
		static void RegisterClass(P param)
		{
			std::ostringstream tmp;
			tmp.str("");
			tmp << typeid(T).name() << "::" << P(param) << std::ends;

			std::string key = tmp.str();
			boost::algorithm::to_lower(key);
			sFactoryMap.insert(std::make_pair(key, new T(param)));
		}
		template <class T>
		static void RegisterClass()
		{
			std::string key = typeid(T).name();

			boost::algorithm::to_lower(key);
			sFactoryMap.insert(std::make_pair(key, new T()));
		}
	public:

		static void* ObjectCreator( std::string& className )
		{
			// search QuantLib classes

			std::string key = "class QuantLib::";
			
			if(className.size() != 0)
				key += className;
			
			boost::algorithm::to_lower(key);
			std::map<std::string, void* >::iterator factoryPos(sFactoryMap.find(key));

			if ( factoryPos != sFactoryMap.end() )
			{
				//boost::shared_ptr<B>* tmp = boost::any_cast<boost::shared_ptr<B> >(&(factoryPos->second));
				//if (tmp)
				//	return *tmp;
				return factoryPos->second;
			}
			
			// search Custom QuantLib classes

			key = "class __QLCPP::";

			if(className.size() != 0)
				key += className;
			
			boost::algorithm::to_lower(key);
			factoryPos = sFactoryMap.find(key);

			if ( factoryPos != sFactoryMap.end() )
			{
				return factoryPos->second;
			}
			
			// no factory found...
			throw std::runtime_error("Attempt to dynamically request instance of unsupported class type");
			//return NULL;
		}

		static void InitObjectFactory();
	};
}





