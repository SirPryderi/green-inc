#!/bin/bash

convert $1 -resize '32x32>' -gravity center -background transparent -extent 32x32 $1
