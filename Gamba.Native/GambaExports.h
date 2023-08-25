#pragma once
#include "Parsing/ShuttingYard.h"

#define GAMBA_EXPORT extern "C" __declspec(dllexport)

class ImmutableManagedVector
{
public:
	template <class T>
	static ImmutableManagedVector* From(const std::vector<T*>* input)
	{
		auto managedVector = new ImmutableManagedVector();
		for (auto item : *input)
		{
			managedVector->items->push_back(item);
		}

		return managedVector;
	}

	template <class T>
	static ImmutableManagedVector* From(const std::vector<const T*>* input)
	{
		return ImmutableManagedVector::From((std::vector<T*>*)input);
	}

	template <class T>
	static ImmutableManagedVector* NonCopyingFrom(std::vector<T>* input)
	{
		auto managedVector = new ImmutableManagedVector();
		managedVector->items = (std::vector<void*>*)input;
		return managedVector;
	}

	int GetCount()
	{
		return items->size();
	}

	void* GetElement(int index)
	{
		return items->at(index);
	}

private:
	std::vector<void*>* items = new std::vector<void*>();
};

GAMBA_EXPORT unsigned int GetVecCount(ImmutableManagedVector* vec)
{
	return vec->GetCount();
}

GAMBA_EXPORT void* GetVecElementAt(ImmutableManagedVector* vec, int index)
{
	return vec->GetElement(index);
}