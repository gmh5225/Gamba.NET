#include "SiMBAPass.h"

#include <chrono>

#include "llvm/IR/Function.h"
#include "llvm/IR/Module.h"
#include "llvm/Pass.h"
#include "llvm/Support/CommandLine.h"
#include "llvm/Support/raw_ostream.h"

#include "llvm/IR/LegacyPassManager.h"
#include "llvm/Transforms/IPO/PassManagerBuilder.h"

#include "LLVMParser.h"

using namespace std;
using namespace llvm;
using namespace std::chrono;

cl::opt<bool>
FastCheck("simbaPassFastCheck", cl::Optional,
    cl::desc("Verify MBA with random values (Default true)"),
    cl::value_desc("simbaPassFastCheck"), cl::init(true));

cl::opt<bool> RunOpt(
    "simbaPassOptimize", cl::Optional,
    cl::desc("Optimize LLVM IR before simplification (Default true)"),
    cl::value_desc("simbaPassOptimize"), cl::init(true));

bool SiMBAPass::runOnModule(Module& M) {
    outs() << "[+] Loading LLVM Module: '" << M.getName() << "'\n";

    LSiMBA::LLVMParser Parser(&M, false, FastCheck, false);

    auto start = high_resolution_clock::now();

    int MBACount = Parser.simplify();

    auto stop = high_resolution_clock::now();
    auto duration = duration_cast<milliseconds>(stop - start);

    outs() << "[+] MBAs found and replaced: '" << MBACount
        << "' time: " << (int)duration.count() << "ms\n";

    return MBACount > 0;
}

bool SiMBAPass::runOnModuleStatic(Module& M) {
    outs() << "[+] Loading LLVM Module: '" << M.getName() << "'\n";

    LSiMBA::LLVMParser Parser(&M, false, FastCheck, false, true, true);

    auto start = high_resolution_clock::now();

    int MBACount = Parser.simplify();

    auto stop = high_resolution_clock::now();
    auto duration = duration_cast<milliseconds>(stop - start);

    outs() << "[+] MBAs found and replaced: '" << MBACount
        << "' time: " << (int)duration.count() << "ms\n";

    return MBACount > 0;
}

char SiMBAPass::ID = 0;

static RegisterPass<SiMBAPass> X("simba", "SiMBA++ MBA simplification pass",
    false /* Only looks at CFG */,
    false /* Analysis Pass */);

// INITIALIZE_PASS(SiMBA, "simba", "SiMBA++ MBA simplification pass", false,
// false)

ModulePass* llvm::createSiMBAPass() { return new SiMBAPass(); }