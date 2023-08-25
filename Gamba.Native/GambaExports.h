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

typedef struct __attribute__((packed))
{
public:
	uint8_t type; //0x0000
	char* str; //0x0001
	int32_t precedence; //0x0009
	bool rightAssociative; //0x000D
	bool unary; //0x000E
	int32_t argIndex; //0x000F
} MarshaledToken;


MarshaledToken* MarshalToken(Gamba::Token token)
{
	auto output = new MarshaledToken();
	output->type = (uint8_t)token.type;
	output->str = _strdup(token.str.c_str());
	output->precedence = token.precedence;
	output->rightAssociative = token.rightAssociative;
	output->unary = token.unary;
	output->argIndex = token.ArgIndex;
	return output;
}


void StringSplit(std::string s, std::string delimiter, std::vector<std::string>& output)
{
	size_t pos = 0;
	std::string token;
	while ((pos = s.find(delimiter)) != std::string::npos) {
		token = s.substr(0, pos);
		output.push_back(token);
		s.erase(0, pos + delimiter.length());
	}

	output.push_back(s);
}

GAMBA_EXPORT void ShuttingYardExport(char* strExpr, char* names, ImmutableManagedVector** outTokens)
{
	// replace ** with p (pow operator is supported by GAMBA)
	auto expr = std::string(strExpr);
	Gamba::replace_all(expr, "**", "#");

	// Parse expressions and create token
	veque::veque<Gamba::Token> tokens;
	std::vector<std::string> varNames;
	StringSplit(names, ",", varNames);

	Gamba::exprToTokens(expr, tokens, true, &varNames);

	// Create deque to parse the operations
	veque::veque<Gamba::Token> queue;
	shuntingYard(tokens, queue);

	// Marshal the tokens to a custom structure.
	auto marshaledTokens = new std::vector<MarshaledToken*>();
	for (auto token : tokens)
		marshaledTokens->push_back(MarshalToken(token));
	*outTokens = ImmutableManagedVector::NonCopyingFrom(marshaledTokens);
}