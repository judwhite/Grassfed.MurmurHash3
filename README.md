<p align="center">
<h2>
<img align="left" width="64" height="64" src="http://judsonwhite.com/images/Grassfed.MurmurHash3.png">
Grassfed.MurmurHash3</h2>
</p>

[![Build status](https://ci.appveyor.com/api/projects/status/qhs43u199n1gpbhy?svg=true)](https://ci.appveyor.com/project/judwhite/grassfed-murmurhash3)&nbsp;&nbsp;[![License](http://img.shields.io/:license-mit-blue.svg)](http://doge.mit-license.org)&nbsp;&nbsp;[![NuGet version](https://badge.fury.io/nu/Grassfed.MurmurHash3.svg)](https://www.nuget.org/packages/Grassfed.MurmurHash3)

MurmurHash3 x64 128-bit - a fast, non-cryptographic hash function.

Port of Austin Appleby's implementation for C#. See https://github.com/aappleby/smhasher/wiki/MurmurHash3 for more information, and [here](https://github.com/aappleby/smhasher/blob/61a0530f28277f2e850bfc39600ce61d02b518de/src/MurmurHash3.cpp#L150) for the original source.

## Install

`PM> Install-Package Grassfed.MurmurHash3`

## Usage

```c#
var bytes = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog.");

var hasher = new MurmurHash3();
var result = hasher.ComputeHash(bytes);

Console.WriteLine(string.Concat(Array.ConvertAll(result, x => x.ToString("x2"))));

// Output: cd99481f9ee902c9695da1a38987b6e7
```

Verify online at http://murmurhash.shorelabs.com. Use the "MurmurHash3" box in the middle, and select the "x64" and "128-bit" dropdown options, otherwise you'll get different results.

## Benchmarks

These benchmarks are meant to highlight the difference in speed for common payload sizes between crypographic and non-cryptographic hash functions.

It's common to see the cryptographic hash functions used for non-cryptographic purposes. They're the only hash functions provided by the .NET standard library and they have a low collission factor by having good distribution.

Many hash function have good distribution without being secure against preimage attacks. If speed matters for your use case there are well reearched options with empirical data to back up the claims. See https://www.strchr.com/hash_functions for more information.

The benchmarks are calculated by running 100,000 iterations of each and taking the average. They're included in the tests project.

Hash algorithm              | Input bytes | MB/s   | ns/op
----------------------------|-----------: |------: |------: 
MurmurHash3 x64 128-bit     | 512         | 3,112  | 157
&nbsp;                      | 4096        | 3,733  | 1,047
&nbsp;                      | 8192        | 3,878  | 2,014
MD5CryptoServiceProvider    | 512         |   135  | 3,604
&nbsp;                      | 4096        |   350  | 11,170
&nbsp;                      | 8192        |   396  | 19,722
SHA1CryptoServiceProvider   | 512         |   124  | 3,921
&nbsp;                      | 4096        |   303  | 12,892
&nbsp;                      | 8192        |   341  | 22,865
SHA256CryptoServiceProvider | 512         |    88  | 5,557
&nbsp;                      | 4096        |   146  | 26,767
&nbsp;                      | 8192        |   154  | 50,699
